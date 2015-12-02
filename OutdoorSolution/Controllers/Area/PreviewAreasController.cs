using System;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Helpers;
using OutdoorSolution.Services.Interfaces;

namespace OutdoorSolution.Controllers
{
    public class PreviewAreasController : PagingController
    {
        private IAreaService areaService;

        public PreviewAreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }

        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            var areas = await areaService.GetPreview(param);
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