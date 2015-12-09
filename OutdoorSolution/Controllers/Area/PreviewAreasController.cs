using System;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Helpers;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Links;

namespace OutdoorSolution.Controllers
{
    public class PreviewAreasController : PagingController
    {
        private IAreaService areaService;
        private AreaLinker areaLinker;

        public PreviewAreasController(IAreaService areaService, AreaLinker areaLinker)
        {
            this.areaService = areaService;
            this.areaLinker = areaLinker;
        }

        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            var areas = await areaService.GetPreview(param);
            areaLinker.LinkifyPreview(areas, Url);
            var responsePage = CreatePage<AreaDto>(areas, param);
            return Ok(responsePage);
        }

        // no need to implement. should never be executed
        public override Task<IHttpActionResult> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override Dto.Infrastructure.Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<PreviewAreasController>(c => c.Get(pagingParams));
        }
    }
}