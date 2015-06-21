﻿using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using OutdoorSolution.Helpers;
using OutdoorSolution.Controllers;
using System.Data.Entity.Spatial;

namespace OutdoorSolution.Mapping
{
    /// <summary>
    /// Creates AreaDto model from Area model and visa versa
    /// </summary>
    public class AreaMapper
    {
        public AreaDto CreateAreaDto(Area area, UrlHelper urlHelper)
        {
            var areaDto = new AreaDto()
            {
                Name = area.Name,
                Description = area.Description,
                Created = area.Created,
                Rating = area.Rating,
                RatingsCount = area.RatingsCount,
                Images = area.Images.Select(x => CreateAreaImageDto(x)).ToList()
            };

            if (urlHelper != null)
            {
                areaDto.Self = urlHelper.Link<AreasController>(c => c.GetById(area.Id));
                // TODO: add this when controllers available
                areaDto.Walls = null;
            }

            if (area.Location != null)
            {
                try
                {
                    areaDto.Location = new GeographyDto()
                    {
                        Longitude = area.Location.Longitude.Value,
                        Latitude = area.Location.Latitude.Value
                    };
                }
                catch (Exception)
                {
                    areaDto.Location = null;
                }
            }

            return areaDto;
        }

        /// <summary>
        /// Creates basic area from dto model. (Not includes statictics fields)
        /// </summary>
        /// <param name="areaDto"></param>
        /// <returns></returns>
        public Area CreateArea(AreaDto areaDto)
        {
            var area = new Area();
            UpdateArea(area, areaDto);
            area.Images = areaDto.Images.Select(x => CreateAreaImage(x)).ToList();
            return area;
        }

        /// <summary>
        /// Updates area's simple and non statistic properties
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaDto"></param>
        public void UpdateArea(Area area, AreaDto areaDto)
        {
            area.Name = areaDto.Name;
            area.Created = areaDto.Created;
            var wellKnownText = String.Format("POINT ({0} {1})", areaDto.Location.Latitude, areaDto.Location.Longitude);
            area.Location = DbGeography.FromText(wellKnownText);
            area.Description = areaDto.Description;
        }

        public AreaImageDto CreateAreaImageDto(AreaImage areaImage)
        {
            return new AreaImageDto()
            {
                Name = areaImage.Name,
                Url = areaImage.Url
            };
        }

        public AreaImage CreateAreaImage(AreaImageDto areaImageDto)
        {
            return new AreaImage()
            {
                Name = areaImageDto.Name,
                Url = areaImageDto.Url
            };
        }
    }
}