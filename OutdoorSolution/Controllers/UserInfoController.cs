using OutdoorSolution.Dal;
using OutdoorSolution.Dto;
using OutdoorSolution.Helpers;
using OutdoorSolution.Links;
using OutdoorSolution.Models;
using OutdoorSolution.Providers;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Web.Http.Description;

namespace OutdoorSolution.Controllers
{
    [Authorize]
    public class UserInfoController : UserResourceController
    {
        private readonly IUserInfoService userInfoService;
        private readonly UserInfoLinker userInfoLinker;
        private readonly IUnitOfWork unitOfWork;

        public UserInfoController(IUserInfoService userInfoService, UserInfoLinker userInfoLinker, IUnitOfWork unitOfWork)
        {
            this.userInfoService = userInfoService;
            this.userInfoLinker = userInfoLinker;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IHttpActionResult> Get(Guid? id = null)
        {
            if (!id.HasValue)
            {
                id = new Guid(User.Identity.GetUserId());
            }

            var userInfo = await userInfoService.GetById(id.Value);
            userInfoLinker.Linkify(userInfo, Url);
            return Ok(userInfo);
        }

        public async Task<IHttpActionResult> Patch(Guid id, [FromBody]UserInfoDto user)
        {
            await userInfoService.Update(id, user);
            await unitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("UserInfo/{userId}/Image")]
        public async Task<IHttpActionResult> PatchImage(Guid userId, UserImageTypes type)
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
                var imageStream = await imageContent.ReadAsStreamAsync();
                var fileExt = ImageHelper.GetImageExtension(imageContent.Headers.ContentType.MediaType);

                if (type == UserImageTypes.Avatar)
                    await userInfoService.UpdateAvatarImage(userId, imageStream, fileExt);
                else if (type == UserImageTypes.Cover)
                    await userInfoService.UpdateCoverImage(userId, imageStream, fileExt);
            }
            else
                return BadRequest("No supported image format found");

            // save updates
            await unitOfWork.SaveChangesAsync();

            return Ok();
        }

        public override void InitUser(string userId)
        {
            this.userInfoService.UserId = userId;
        }

        [ApiExplorerSettings(IgnoreApi=true), NonAction]
        public override async Task<IHttpActionResult> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override Dto.Infrastructure.Link GetPagingLink(PagingParams pagingParams)
        {
            throw new NotImplementedException();
        }
    }
}
