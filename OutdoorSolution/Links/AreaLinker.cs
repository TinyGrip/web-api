using OutdoorSolution.Dto;
using System;
using System.Linq;
using System.Web.Http.Routing;
using OutdoorSolution.Helpers;
using OutdoorSolution.Controllers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace OutdoorSolution.Links
{
    /// <summary>
    /// Creates AreaDto model from Area model and visa versa
    /// </summary>
    public class AreaLinker
    {
        private readonly WallLinker wallLinker;
        private readonly RouteLinker routeLinker;

        public AreaLinker(WallLinker wallMapper, RouteLinker routeMapper)
        {
            this.wallLinker = wallMapper;
            this.routeLinker = routeMapper;
        }

        public void Linkify(AreaDto area, UrlHelper urlHelper)
        {
            area.Self = urlHelper.Link<AreasController>(c => c.GetById(area.Id));
            area.Walls = urlHelper.Link<WallsController>(c => c.Get(area.Id, null));
            area.AddWall = urlHelper.Link<WallsController>(c => c.Post(area.Id, null));

            area.Images.ToList().ForEach(ai => Linkify(ai, urlHelper));
            area.PreviewWalls.ToList().ForEach(w => wallLinker.Linkify(w, urlHelper));
            area.PreviewRoutes.ToList().ForEach(r => routeLinker.Linkify(r, urlHelper));
        }
        
        public void Linkify(IEnumerable<AreaDto> areas, UrlHelper urlHelper)
        {
            foreach(var area in areas)
            {
                Linkify(area, urlHelper);
            }
        }

        public void LinkifyPreview(AreaDto area, UrlHelper urlHelper)
        {
            // Creating a link to AreaController in order to get full entry
            area.Self = urlHelper.Link<AreasController>(c => c.GetById(area.Id));
        }

        public void Linkify(AreaImageDto areaImage, UrlHelper urlHelper)
        {
            areaImage.Link = new Link()
            {
                Href = new Uri(areaImage.Href),
                Templated = false
            };

            areaImage.Href = null;
        }
    }
}