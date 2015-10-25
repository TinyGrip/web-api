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
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dal;
using OutdoorSolution.Mapping;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Filters;
using OutdoorSolution.Services;

namespace OutdoorSolution.Controllers
{
    public class AreasController : UserResourceController<Area, AreaDto>
    {
        private readonly AreaMapper areaMapper;

        public AreasController(ApplicationDbContext dbContext, AreaMapper areaMapper, PermissionsService permissionsService)
            : base(dbContext, permissionsService)
        {
            this.areaMapper = areaMapper;
        }

        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            // TODO: check if to use eager loading!
            param.TotalAmount = db.Areas.Count();
            if (param.TotalAmount == 0)
            {
                return NotFound();
            }

            var areas = await db.Areas.OrderByDescending(a => a.Name)
                                      .Skip(param.Skip)
                                      .Take(param.Take)
                                      .ToListAsync();

            // TODO: think about memory
            var areaDtos = areas.Select(a => CreateDto(a)).ToList();
            areas = null;

            var responsePage = CreatePage<AreaDto>(areaDtos, param);
            return Ok(responsePage);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostArea(AreaDto areaDto)
        {
            areaDto.Created = DateTime.Now;

            // create db model and save it
            var area = areaMapper.CreateArea(areaDto);
            db.Areas.Add(area);
            await db.SaveChangesAsync();

            // create dto model of saved area
            areaDto = CreateDto(area);

            return CreatedAtRoute("DefaultApi", new { id = area.Id }, areaDto);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<AreasController>(c => c.Get(pagingParams));
        }

        protected override AreaDto CreateDto(Area resource)
        {
            return areaMapper.CreateAreaDto(resource, Url);
        }

        protected override void Update(Area resource, AreaDto resourceDto)
        {
            areaMapper.UpdateArea(resource, resourceDto);
        }
    }
}