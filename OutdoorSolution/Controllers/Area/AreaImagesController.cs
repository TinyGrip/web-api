using System;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Interfaces;
using System.Net;
using OutdoorSolution.Providers;
using OutdoorSolution.Helpers;
using System.Net.Http;
using OutdoorSolution.Links;

namespace OutdoorSolution.Controllers
{
    [Route(AREA_IMAGE_ROUTE)]
    public class AreaImagesController : UserResourceController
    {
        private const string AREA_IMAGE_ROUTE = "Areas/Images/{id}";

        IAreaImageService aiService;
        AreaImageLinker aiLinker;

        public AreaImagesController(IAreaImageService areaImageService, AreaImageLinker aiLinker)
        {
            this.aiService = areaImageService;
            this.aiLinker = aiLinker;
        }

        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var areaImage = await aiService.GetById(id);
            aiLinker.Linkify(areaImage, Url);
            return Ok(areaImage);
        }

        [Route("Areas/{areaId}/Images")]
        public async Task<IHttpActionResult> GetByAreaId(Guid areaId)
        {
            var areaImagesDtos = await aiService.GetByArea(areaId);
            aiLinker.Linkify(areaImagesDtos, Url);
            return Ok(areaImagesDtos);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostAreaImage(Guid areaId, [FromBody]AreaImageDto areaImage)
        {
            var areaImageWrapper = aiService.Create(areaId, areaImage);

            await UnitOfWork.SaveChangesAsync();

            areaImage = await areaImageWrapper.GetValue();
            aiLinker.Linkify(areaImage, Url);
            return Created(String.Empty, areaImage);
        }

        [Authorize]
        public async Task<IHttpActionResult> PatchAreaImage(Guid areaImageId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // read the form data
            var provider = new MultipartImageStreamsProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var imageContent = provider.Contents[0];
            if (imageContent != null)
            {
                await aiService.UpdateImage(
                    areaImageId,
                    await imageContent.ReadAsStreamAsync(),
                    ImageHelper.GetImageExtension(imageContent.Headers.ContentType.MediaType));
            }
            else
                return BadRequest("No supported image format found");

            // save updates
            await UnitOfWork.SaveChangesAsync();

            return Ok();
        }

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