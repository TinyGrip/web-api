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
using OutdoorSolution.Mapping;
using OutdoorSolution.Dto;
using OutdoorSolution.Models;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services;

namespace OutdoorSolution.Controllers
{
    public class RoutesController : UserResourceController<Route, RouteDto>
    {
        private readonly RouteMapper routeMapper;
        private Guid wallId;

        public RoutesController(ApplicationDbContext dbContext, PermissionsService permissionsService, RouteMapper routeMapService)
            : base(dbContext, permissionsService)
        {
            this.routeMapper = routeMapService;
        }

        public async Task<IHttpActionResult> Get(Guid wallId, [FromUri]PagingParams param)
        {
            var q = db.Routes.Where(x => x.WallId == wallId);
            param.TotalAmount = q.Count();
            if (param.TotalAmount == 0)
            {
                return NotFound();
            }

            var routes = await q.OrderByDescending(x => x.Name)
                                      .Skip(param.Skip)
                                      .Take(param.Take)
                                      .ToListAsync();

            this.wallId = wallId;

            var routesDto = routes.Select(x => routeMapper.CreateRouteDto(x, Url));
            
            var page = CreatePage(routesDto, param);
            return Ok(page);
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(Guid wallId, [FromBody]RouteDto routeDto)
        {
            var route = routeMapper.CreateRoute(routeDto);
            route.WallId = wallId;

            db.Routes.Add(route);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = route.Id }, routeDto);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<RoutesController>(c => c.Get(wallId, pagingParams));
        }

        protected override RouteDto CreateDto(Route resource)
        {
            return routeMapper.CreateRouteDto(resource, Url);
        }

        protected override void Update(Route resource, RouteDto resourceDto)
        {
            routeMapper.UpdateRoute(resource, resourceDto);
        }
    }
}