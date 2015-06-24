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

namespace OutdoorSolution.Controllers
{
    public class WallsController : PagingController<WallsController>
    {
        private readonly ApplicationDbContext db;
        private readonly WallMapService wallMapper;

        public WallsController(ApplicationDbContext dbContext, WallMapService wallMapper)
        {
            db = dbContext;
            this.wallMapper = wallMapper;
        }
        
        public async Task<IHttpActionResult> Get(Guid areaId, PagingParams param) 
        {
            param = param ?? DefaultPagingParams;

            var walls = await db.Walls.OrderByDescending(a => a.Name)
                                      .Skip(param.Skip)
                                      .Take(param.Take)
                                      .ToListAsync();

            if (walls.Count == 0)
                return NotFound();

            var wallsDtos = walls.Select(w => wallMapper.CreateWallDto(w)).ToList();
            param.TotalAmount = wallsDtos.Count;
            walls = null;


            var responsePage = CreatePage<WallDto>(wallsDtos, param);
            return Ok(responsePage);
        }

        [ResponseType(typeof(Wall))]
        public override async Task<IHttpActionResult> GetById(Guid id)
        {
            Wall wall = await db.Walls.FindAsync(id);
            if (wall == null)
            {
                return NotFound();
            }

            var wallDto = wallMapper.CreateWallDto(wall);

            return Ok(wallDto);
        }

        // PUT: api/Walls/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWall(Guid id, Wall wall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wall.Id)
            {
                return BadRequest();
            }

            db.Entry(wall).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WallExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Walls
        [ResponseType(typeof(Wall))]
        public async Task<IHttpActionResult> PostWall(Guid areaId, WallDto wallDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wall = wallMapper.CreateWall(wallDto);
            db.Walls.Add(wall);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = wall.Id }, wall);
        }

        // DELETE: api/Walls/5
        [ResponseType(typeof(Wall))]
        public async Task<IHttpActionResult> DeleteWall(Guid id)
        {
            Wall wall = await db.Walls.FindAsync(id);
            if (wall == null)
            {
                return NotFound();
            }

            db.Walls.Remove(wall);
            await db.SaveChangesAsync();

            return Ok(wall);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WallExists(Guid id)
        {
            return db.Walls.Count(e => e.Id == id) > 0;
        }
    }
}