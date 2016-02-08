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
    public class RouteLinker : ILinker
    {
        public void Linkify(RouteDto route, UrlHelper urlHelper)
        {
            route.Self = urlHelper.Link<RoutesController>(c => c.GetById(route.Id));
            route.Wall = urlHelper.Link<WallsController>(c => c.GetById(route.WallId));
            route.Comments = urlHelper.Link<CommentsController>(c => c.GetByRoute(route.Id, null));

            if (route.Permissions.CanModify)
                route.Update = urlHelper.Link<RoutesController>(c => c.Patch(route.Id, null));
            if (route.Permissions.CanDelete)
                route.Delete = urlHelper.Link<RoutesController>(c => c.Delete(route.Id));
            if (route.CanComment)
                route.AddComment = urlHelper.Link<CommentsController>(a => a.PostByArea(route.Id, null));
        }

        public void Linkify(IEnumerable<RouteDto> routes, UrlHelper urlHelper)
        {
            foreach (var route in routes)
            {
                Linkify(route, urlHelper);
            }
        }
    }
}