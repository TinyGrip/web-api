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

namespace OutdoorSolution.Links
{
    public class WallLinker
    {
        public void Linkify(WallDto wall, UrlHelper urlHelper)
        {
            wall.Self = urlHelper.Link<WallsController>(c => c.GetById(wall.Id));
            wall.Update = urlHelper.Link<WallsController>(c => c.Put(wall.Id, null));
            wall.Delete = urlHelper.Link<WallsController>(c => c.Delete(wall.Id));
            wall.Area = urlHelper.Link<AreasController>(c => c.GetById(wall.AreaId));
            wall.Routes = urlHelper.Link<RoutesController>(c => c.Get(wall.Id, null));

            if (!String.IsNullOrEmpty(wall.ImageHref))
            {
                wall.Image = new Link()
                {
                    Href = ImageHelper.GetImageUri(wall.ImageHref, urlHelper.Request.RequestUri),
                    Templated = false
                };

                wall.ImageHref = null;
            }
        }

        public void Linkify(IEnumerable<WallDto> walls, UrlHelper urlHelper)
        {
            foreach (var wall in walls)
            {
                Linkify(wall, urlHelper);
            }
        }
    }
}