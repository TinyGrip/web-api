using Microsoft.AspNet.Identity;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Helpers;
using OutdoorSolution.Links;
using OutdoorSolution.Models;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace OutdoorSolution.Controllers
{
    public class AreasController : UserResourceController
    {
        private readonly IAreaService areaService;
        private readonly AreaLinker areaLinker;

        public AreasController(IAreaService areaService, AreaLinker areaLinker)
        {
            this.areaService = areaService;
            this.areaService.UserId = User.Identity.GetUserId();
            this.areaLinker = areaLinker;
        }

        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var area = await areaService.GetById(id);
            areaLinker.Linkify(area, Url);
            return Ok(area);
        }

        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            var areas = await areaService.Get(param);
            areaLinker.Linkify(areas, Url);

            var responsePage = CreatePage<AreaDto>(areas, param);
            return Ok(responsePage);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostArea(AreaDto areaDto)
        {
            var areaWrapper = areaService.Create(areaDto);
            await UnitOfWork.SaveChangesAsync();

            var area = await areaWrapper.GetValue();
            areaLinker.Linkify(area, Url);

            return Created(area.Self.Href, areaDto);
        }

        [Authorize]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]AreaDto areaDto)
        {
            await areaService.Update(id, areaDto);
            await UnitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await areaService.Delete(id);
            await UnitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<AreasController>(c => c.Get(pagingParams));
        }

        public override void InitUser(string userId)
        {
            this.areaService.UserId = userId;
        }
    }
}