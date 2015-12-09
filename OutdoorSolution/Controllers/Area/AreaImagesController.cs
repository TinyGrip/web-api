using System;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace OutdoorSolution.Controllers
{
    [Route("api/Areas/{areaId}/Images")]
    public class AreaImagesController : UserResourceController
    {
        private const string AREA_IMAGE_ROUTE = "api/Areas/Images/{id}";

        IAreaImageService aiService;

        public AreaImagesController(IAreaImageService areaImageService)
        {
            this.aiService = areaImageService;
            this.aiService.UserId = User.Identity.GetUserId();
        }

        [Route(AREA_IMAGE_ROUTE)]
        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var areaImage = await aiService.GetById(id);
            return Ok(areaImage);
        }

        public async Task<IHttpActionResult> GetByAreaId(Guid areaId)
        {
            var areaImagesDtos = await aiService.GetByArea(areaId);
            return Ok(areaImagesDtos);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostAreaImage(Guid areaId, [FromBody]AreaImageDto areaImageDto)
        {
            var areaImageWrapper = aiService.Create(areaId, areaImageDto);

            await UnitOfWork.SaveChangesAsync();

            areaImageDto = await areaImageWrapper.GetValue();
            return Created(String.Empty, areaImageDto);
        }

        [Route(AREA_IMAGE_ROUTE)]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAreaImage(Guid id)
        {
            await aiService.Delete(id);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        public override void InitUser(string userId)
        {
            this.aiService.UserId = userId;
        }

        protected override Dto.Infrastructure.Link GetPagingLink(Models.PagingParams pagingParams)
        {
            throw new NotImplementedException();
        }
    }
}