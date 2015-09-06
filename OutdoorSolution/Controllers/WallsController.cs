using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Mapping;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Models;

namespace OutdoorSolution.Controllers
{
    public class WallsController : PagingController<WallsController>
    {
        private readonly ApplicationDbContext db;
        private readonly WallMapper wallMapper;
        private Guid? parentAreaId;

        public WallsController(ApplicationDbContext dbContext, WallMapper wallMapper)
        {
            db = dbContext;
            this.wallMapper = wallMapper;
        }

        public async Task<IHttpActionResult> Get(Guid areaId, [FromUri]PagingParams param) 
        {
            var q =  db.Walls.Where(w => w.AreaId == areaId);                        
            param.TotalAmount = q.Count();

            if (param.TotalAmount == 0)
                return NotFound();

            var walls = await q.OrderByDescending(a => a.Name)
                               .Skip(param.Skip)
                               .Take(param.Take)
                               .ToListAsync();

            this.parentAreaId = areaId;

            var wallsDtos = walls.Select(w => wallMapper.CreateWallDto(w, Url)).ToList();
            walls = null;

            var responsePage = CreatePage<WallDto>(wallsDtos, param);
            return Ok(responsePage);
        }

        public override async Task<IHttpActionResult> GetById(Guid id)
        {
            Wall wall = await db.Walls.FindAsync(id);
            if (wall == null)
            {
                return NotFound();
            }

            var wallDto = wallMapper.CreateWallDto(wall, Url);

            return Ok(wallDto);
        }

        [Authorize]
        public async Task<IHttpActionResult> PutWall(Guid id, [FromBody]WallDto wallDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wall = await db.Walls.FindAsync(id);
            if (wall == null)
            {
                return NotFound();
            }

            wallMapper.UpdateWall(wall, wallDto);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostWall(Guid areaId, [FromBody]WallDto wallDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wall = wallMapper.CreateWall(wallDto);
            wall.AreaId = areaId;
            
            db.Walls.Add(wall);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = wall.Id }, wallMapper.CreateWallDto(wall, Url));
        }

        [Authorize]
        public async Task<IHttpActionResult> DeleteWall(Guid id)
        {
            Wall wall = await db.Walls.FindAsync(id);
            if (wall == null)
            {
                return NotFound();
            }

            db.Walls.Remove(wall);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            if (this.parentAreaId.HasValue)
                return Url.Link<WallsController>(c => c.Get(this.parentAreaId.Value, pagingParams));
            else
                //return Url.Link<WallsController>(c => c.Get(pagingParams));
                return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}