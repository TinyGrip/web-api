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
    public class WallMapService
    {
        public WallDto CreateWallDto(Wall wall, UrlHelper urlHelper)
        {
            var wallDto = new WallDto()
            {
                Name = wall.Name,
                Image = new Link()
                {
                    Href = new Uri(wall.Image),
                    Templated = false
                },
                Location = Utils.CreateGeoDto(wall.Location)
            };

            if (urlHelper != null)
            {
                wallDto.Self = urlHelper.Link<WallsController>(c => c.GetById(wall.Id));
                wallDto.Area = urlHelper.Link<AreasController>(c => c.GetById(wall.AreaId));
                //wallDto.Routes = urlHelper.Link<RouteController>(c => c.Get(wall.Id, null));
            }

            return wallDto;
        }

        public Wall CreateWall(WallDto wallDto)
        {
            var wall = new Wall()
            {
                Name = wallDto.Name,
                Image = wallDto.ImageHref,
                Location = Utils.CreateDbPoint(wallDto.Location)
            };

            return wall;
        }
    }
}