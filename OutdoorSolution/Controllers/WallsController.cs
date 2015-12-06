using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Models;
using OutdoorSolution.Providers;
using Microsoft.AspNet.Identity;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Links;

namespace OutdoorSolution.Controllers
{
    public class WallsController : UserResourceController<Wall, WallDto>
    {
        private readonly IWallService wallService;
        private Guid? parentAreaId;
        private readonly WallLinker wallLinker;

        public WallsController(IWallService wallService, WallLinker wallLinker)
        {
            this.wallService = wallService;
            this.wallService.UserId = User.Identity.GetUserId();
            this.wallLinker = wallLinker;
        }

        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var wall = await wallService.GetById(id);
            wallLinker.Linkify(wall, Url);
            return Ok(wall);
        }

        public async Task<IHttpActionResult> Get(Guid areaId, [FromUri]PagingParams param) 
        {  
            this.parentAreaId = areaId;
            var walls = await wallService.Get(areaId, param);
            wallLinker.Linkify(walls, Url);

            var responsePage = CreatePage<WallDto>(walls, param);
            return Ok(responsePage);
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(Guid areaId, [FromBody]WallDto wallDto)
        {
            var wallWrapper = wallService.Create(areaId, wallDto);
            await UnitOfWork.SaveChangesAsync();

            var wall = wallWrapper.GetValue();
            wallLinker.Linkify(wall, Url);

            return Created(String.Empty, wall);
        }

        [Authorize]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]WallDto wallDto)
        {
            await wallService.Update(id, wallDto);
            await UnitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        [Route("api/Walls/{wallId}/Image")]
        public async Task<IHttpActionResult> PatchWallImage(Guid wallId)
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
                await wallService.UpdateImage(
                    wallId,
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
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await wallService.Delete(id);
            await UnitOfWork.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            if (this.parentAreaId.HasValue)
                return Url.Link<WallsController>(c => c.Get(this.parentAreaId.Value, pagingParams));
            else
                //return Url.Link<WallsController>(c => c.Get(pagingParams));
                return null;
        }
    }
}