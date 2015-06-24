using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutdoorSolution.Mapping
{
    public class WallMapService
    {
        public WallDto CreateWallDto(Wall wall)
        {
            var wallDto = new WallDto()
            {
                Name = wall.Name,
                Image = wall.Image
            };

            if (wall.Location != null && wall.Location.Longitude.HasValue && wall.Location.Latitude.HasValue)
            {
                wallDto.Location = new GeographyDto()
                {
                    Longitude = wall.Location.Longitude.Value,
                    Latitude = wall.Location.Latitude.Value
                };
            }

            return wallDto;
        }

        public Wall CreateWall(WallDto wallDto)
        {
            var wall = new Wall()
            {
                Name = wallDto.Name,
                Image = wallDto.Image,
                Location = Utils.CreateDbPoint(wallDto.Location)
            };

            return wall;
        }
    }
}