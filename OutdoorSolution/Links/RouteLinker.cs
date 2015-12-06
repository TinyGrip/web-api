using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using OutdoorSolution.Helpers;
using OutdoorSolution.Controllers;

namespace OutdoorSolution.Links
{
    public class RouteLinker
    {
        public void Linkify(RouteDto route, UrlHelper urlHelper)
        {
            route.Self = urlHelper.Link<RoutesController>(c => c.GetById(route.Id));
            route.Wall = urlHelper.Link<WallsController>(c => c.GetById(route.WallId));
            route.Update = urlHelper.Link<RoutesController>(c => c.Put(route.Id, null));
            route.Delete = urlHelper.Link<RoutesController>(c => c.Delete(route.Id));
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