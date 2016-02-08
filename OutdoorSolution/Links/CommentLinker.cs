using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using OutdoorSolution.Helpers;
using OutdoorSolution.Controllers;
using OutdoorSolution.Services.Interfaces;
using System.Threading.Tasks;

namespace OutdoorSolution.Links
{
    /// <summary>
    /// Creates hypermedia links on route dto object
    /// </summary>
    public class CommentLinker : ILinker
    {
        public void LinkifyAreaComment(CommentDto comment, UrlHelper urlHelper)
        {
            if (comment.Permissions.CanDelete)
            {
                comment.Delete = urlHelper.Link<CommentsController>(c => c.DeleteAreaComment(comment.Id));
            }
        }

        public void LinkifyAreaComments(IEnumerable<CommentDto> comments, UrlHelper urlHelper)
        {
            foreach (var comment in comments)
            {
                LinkifyAreaComment(comment, urlHelper);
            }
        }

        public void LinkifyRouteComment(CommentDto comment, UrlHelper urlHelper)
        {
            if (comment.Permissions.CanDelete)
            {
                comment.Delete = urlHelper.Link<CommentsController>(c => c.DeleteRouteComment(comment.Id));
            }
        }

        public void LinkifyRouteComments(IEnumerable<CommentDto> comments, UrlHelper urlHelper)
        {
            foreach (var comment in comments)
            {
                LinkifyRouteComment(comment, urlHelper);
            }
        }
    }
}