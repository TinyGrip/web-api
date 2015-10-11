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

namespace OutdoorSolution.Controllers
{
    public class RoutesController : PagingController
    {
        private readonly ApplicationDbContext db;
        private readonly RouteMapper routeMapper;
        private Guid wallId;

        public RoutesController(ApplicationDbContext dbContext, RouteMapper routeMapService)
        {
            db = dbContext;
            this.routeMapper = routeMapService;
        }
    
        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            var routeDto = routeMapper.CreateRouteDto(route, Url);

            return Ok(routeDto);
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
        public async Task<IHttpActionResult> PutRoute(Guid id, [FromBody]RouteDto routeDto)
        {
            var route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return BadRequest();
            }

            routeMapper.UpdateRoute(route, routeDto);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostRoute(Guid wallId, [FromBody]RouteDto routeDto)
        {
            var route = routeMapper.CreateRoute(routeDto);
            route.WallId = wallId;

            db.Routes.Add(route);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = route.Id }, routeDto);
        }

        [Authorize]
        public async Task<IHttpActionResult> DeleteRoute(Guid id)
        {
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            db.Routes.Remove(route);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<RoutesController>(c => c.Get(wallId, pagingParams));
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