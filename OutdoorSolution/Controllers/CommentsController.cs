using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Dal;
using System.Net;
using OutdoorSolution.Links;

namespace OutdoorSolution.Controllers
{
    public class CommentsController : UserResourceController
    {
        readonly ICommentService commentService;
        readonly CommentLinker commentLinker;
        Guid? currentRouteId, currentAreaId;

        public CommentsController(ICommentService commentService, CommentLinker commentLinker)
        {
            this.commentService = commentService;
            this.commentLinker = commentLinker;
        }

        [Route("Areas/{areaId}/Comments")]
        public async Task<IHttpActionResult> GetByArea(Guid areaId, [FromUri] PagingParams paging)
        {   
            var comments = await commentService.GetByArea(areaId, paging);
            commentLinker.LinkifyAreaComments(comments, Url);
            var page = CreatePage(comments, paging);
            this.currentAreaId = areaId;
            return Ok(page);
        }

        [Route("Routes/{routeId}/Comments")]
        public async Task<IHttpActionResult> GetByRoute(Guid routeId, [FromUri] PagingParams paging)
        {
            var comments = await commentService.GetByRoute(routeId, paging);
            commentLinker.LinkifyRouteComments(comments, Url);
            var page = CreatePage(comments, paging);
            this.currentRouteId = routeId;
            return Ok(page);
        }

        [Authorize]
        [Route("Areas/{areaId}/Comments")]
        public async Task<IHttpActionResult> PostByArea(Guid areaId, [FromBody]CommentDto commentDto)
        {
            var commentWrapper = commentService.CreateForArea(areaId, commentDto);
            await UnitOfWork.SaveChangesAsync();

            commentDto = await commentWrapper.GetValue();
            commentLinker.LinkifyAreaComment(commentDto, Url);

            return Created(String.Empty, commentDto);
        }

        [Authorize]
        [Route("Routes/{routeId}/Comments")]
        public async Task<IHttpActionResult> PostByRoute(Guid routeId, [FromBody]CommentDto commentDto)
        {
            var commentWrapper = commentService.CreateForRoute(routeId, commentDto);
            await UnitOfWork.SaveChangesAsync();

            commentDto = await commentWrapper.GetValue();
            commentLinker.LinkifyRouteComment(commentDto, Url);

            return Created(String.Empty, commentDto);
        }

        [Authorize(Roles = RoleNames.Admin)]
        [Route("Areas/Comments/{id}")]
        public async Task<IHttpActionResult> DeleteAreaComment(Guid id)
        {
            await commentService.DeleteAreaComment(id);
            await UnitOfWork.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = RoleNames.Admin)]
        [Route("Routes/Comments/{id}")]
        public async Task<IHttpActionResult> DeleteRouteComment(Guid id)
        {
            await commentService.DeleteRouteComment(id);
            await UnitOfWork.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [NonAction]
        public override Task<IHttpActionResult> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            if (currentAreaId.HasValue)
                return Url.Link<CommentsController>(c => c.GetByArea(currentAreaId.Value, pagingParams));

            else if (currentRouteId.HasValue)
                return Url.Link<CommentsController>(c => c.GetByArea(currentRouteId.Value, pagingParams));

            else
                return null;
        }

        public override void InitUser(string userId)
        {
            commentService.UserId = userId;
        }
    }
}
