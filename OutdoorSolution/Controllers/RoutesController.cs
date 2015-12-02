using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Models;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using Microsoft.AspNet.Identity;
using OutdoorSolution.Services.Interfaces;

namespace OutdoorSolution.Controllers
{
    public class RoutesController : UserResourceController<Route, RouteDto>
    {
        private readonly IRouteService routeService;
        private Guid wallId;

        public RoutesController(IRouteService routeService)
        {
            this.routeService = routeService;
            this.routeService.UserId = User.Identity.GetUserId();
        }

        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var route = await routeService.GetById(id);
            return Ok(route);
        }

        public async Task<IHttpActionResult> Get(Guid wallId, [FromUri]PagingParams param)
        {
            this.wallId = wallId;
            var routes = await routeService.Get(wallId, param);
            
            var page = CreatePage(routes, param);
            return Ok(page);
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(Guid wallId, [FromBody]RouteDto routeDto)
        {
            this.wallId = wallId;
            var routeWrapper = routeService.Create(wallId, routeDto);
            await UnitOfWork.SaveChangesAsync();

            var route = routeWrapper.GetValue();

            return Created(String.Empty, route);
        }

        public async Task<IHttpActionResult> Put(Guid id, [FromBody]RouteDto routeDto)
        {
            await routeService.Update(id, routeDto);
            await UnitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await routeService.Delete(id);
            await UnitOfWork.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<RoutesController>(c => c.Get(wallId, pagingParams));
        }
    }
}