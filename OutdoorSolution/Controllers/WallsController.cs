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
using OutdoorSolution.Services;

namespace OutdoorSolution.Controllers
{
    public class WallsController : UserResourceController<Wall, WallDto>
    {
        private readonly WallMapper wallMapper;
        private Guid? parentAreaId;

        public WallsController(ApplicationDbContext dbContext, PermissionsService permissionsService, WallMapper wallMapper)
            : base(dbContext, permissionsService)
        {
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

        [Authorize]
        public async Task<IHttpActionResult> Post(Guid areaId, [FromBody]WallDto wallDto)
        {
            var wall = wallMapper.CreateWall(wallDto);
            wall.AreaId = areaId;

            db.Walls.Add(wall);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = wall.Id }, wallMapper.CreateWallDto(wall, Url));
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            if (this.parentAreaId.HasValue)
                return Url.Link<WallsController>(c => c.Get(this.parentAreaId.Value, pagingParams));
            else
                //return Url.Link<WallsController>(c => c.Get(pagingParams));
                return null;
        }

        protected override WallDto CreateDto(Wall resource)
        {
            return wallMapper.CreateWallDto(resource, Url);
        }

        protected override void Update(Wall resource, WallDto resourceDto)
        {
            wallMapper.UpdateWall(resource, resourceDto);
        }
    }
}