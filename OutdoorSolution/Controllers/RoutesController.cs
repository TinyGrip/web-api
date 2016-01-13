using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Dto;
using OutdoorSolution.Models;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using Microsoft.AspNet.Identity;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Links;

namespace OutdoorSolution.Controllers
{
    public class RoutesController : UserResourceController
    {
        private readonly IRouteService routeService;
        private Guid wallId;
        private readonly RouteLinker routeLinker;

        public RoutesController(IRouteService routeService, RouteLinker routeLinker)
        {
            this.routeService = routeService;
            this.routeService.UserId = User.Identity.GetUserId();
            this.routeLinker = routeLinker;
        }

        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var route = await routeService.GetById(id);
            routeLinker.Linkify(route, Url);
            return Ok(route);
        }

        public async Task<IHttpActionResult> Get(Guid wallId, [FromUri]PagingParams param)
        {
            this.wallId = wallId;
            var routes = await routeService.Get(wallId, param);
            routeLinker.Linkify(routes, Url);
            
            var page = CreatePage(routes, param);
            return Ok(page);
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(Guid wallId, [FromBody]RouteDto routeDto)
        {
            this.wallId = wallId;
            var routeWrapper = routeService.Create(wallId, routeDto);
            await UnitOfWork.SaveChangesAsync();

            var route = await routeWrapper.GetValue();
            routeLinker.Linkify(route, Url);

            return Created(route.Self.Href, route);
        }

        public async Task<IHttpActionResult> Patch(Guid id, [FromBody]RouteDto routeDto)
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

        public override void InitUser(string userId)
        {
            this.routeService.UserId = userId;
        }
    }
}