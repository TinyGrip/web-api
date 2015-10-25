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

namespace OutdoorSolution.Mapping
{
    public class RouteMapper
    {
        public RouteDto CreateRouteDto(Route route, UrlHelper urlHelper)
        {
            var routeDto = new RouteDto()
            {
                Name = route.Name,
                Type = route.Type,
                Complexity = route.Complexity,
                Path = Utils.ConvertDbGeometry(route.Path)
            };

            if (urlHelper != null)
            {
                routeDto.Self = urlHelper.Link<RoutesController>(c => c.GetById(route.Id));
                routeDto.Wall = urlHelper.Link<WallsController>(c => c.GetById(route.WallId));
                routeDto.Update = urlHelper.Link<RoutesController>(c => c.Put(route.Id, null));
                routeDto.Delete = urlHelper.Link<RoutesController>(c => c.Delete(route.Id));
            }

            return routeDto;
        }

        public Route CreateRoute(RouteDto routeDto)
        {
            var route = new Route();
            UpdateRoute(route, routeDto);
            return route;
        }

        public void UpdateRoute(Route route, RouteDto routeDto)
        {
            route.Name = routeDto.Name;
            route.Type = routeDto.Type;
            route.Complexity = routeDto.Complexity;
            route.Path = Utils.CreateDbGeometry(routeDto.Path);
        }
    }
}