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
using OutdoorSolution.Dto.Infrastructure;

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
                wallDto.Update = urlHelper.Link<WallsController>(c => c.PutWall(wall.Id, null));
                wallDto.Delete = urlHelper.Link<WallsController>(c => c.DeleteWall(wall.Id));
                wallDto.Area = urlHelper.Link<AreasController>(c => c.GetById(wall.AreaId));
                wallDto.Routes = urlHelper.Link<RoutesController>(c => c.Get(wall.Id, null));
            }

            return wallDto;
        }

        public Wall CreateWall(WallDto wallDto)
        {
            var wall = new Wall();
            UpdateWall(wall, wallDto);
            return wall;
        }

        public void UpdateWall(Wall wall, WallDto wallDto)
        {
            wall.Name = wallDto.Name;
            wall.Image = wallDto.ImageHref;
            wall.Location = Utils.CreateDbPoint(wallDto.Location);
        }
    }
}