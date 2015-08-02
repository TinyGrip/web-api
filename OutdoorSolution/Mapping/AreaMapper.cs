using OutdoorSolution.Domain.Models;
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
using OutdoorSolution.Services.Common;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Domain.Models.Infrastructure;

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
                areaDto.Walls = urlHelper.Link<WallsController>(c => c.Get(area.Id, null));
                areaDto.AddWall = urlHelper.Link<WallsController>(c => c.PostWall(area.Id, null));
            }

            areaDto.Location = Utils.CreateGeoDto(area.Location);

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
            if (areaDto.Images != null)
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
            area.Location = Utils.CreateDbPoint(areaDto.Location);
            area.Description = areaDto.Description;
        }

        public AreaDto CreatePreviewAreaDto(PreviewArea area, UrlHelper urlHelper)
        {
            var areaDto = new AreaDto()
            {
                Name = area.Name,
                Rating = area.Rating
            };

            if (urlHelper != null)
            {
                // Creating a link to AreaController in order to get full entry
                areaDto.Self = urlHelper.Link<AreasController>(c => c.GetById(area.Id));
            }

            areaDto.Location = Utils.CreateGeoDto(area.Location);
            return areaDto;
        }

        public AreaImageDto CreateAreaImageDto(AreaImage areaImage)
        {
            return new AreaImageDto()
            {
                Name = areaImage.Name,
                Link = new Link()
                {
                    Href = new Uri(areaImage.Url),
                    Templated = false
                }
            };
        }

        public AreaImage CreateAreaImage(AreaImageDto areaImageDto)
        {
            return new AreaImage()
            {
                Name = areaImageDto.Name,
                Url = areaImageDto.Href
            };
        }
    }
}