using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Web.Http.Routing;
using OutdoorSolution.Helpers;
using OutdoorSolution.Controllers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Services.Common;
using System.Threading.Tasks;

namespace OutdoorSolution.Links
{
    /// <summary>
    /// Creates hypermedia link on wall dto object
    /// </summary>
    public class WallLinker : ILinker
    {
        public void Linkify(WallDto wall, UrlHelper urlHelper)
        {
            wall.Self = urlHelper.Link<WallsController>(c => c.GetById(wall.Id));
            wall.Area = urlHelper.Link<AreasController>(c => c.GetById(wall.AreaId));
            wall.Routes = urlHelper.Link<RoutesController>(c => c.Get(wall.Id, null));
            
            if (!String.IsNullOrEmpty(wall.ImageHref))
            {
                wall.Image = ImageHelper.GetImageLink(wall.ImageHref, urlHelper.Request.RequestUri);
                wall.ImageHref = null;
            }

            if (wall.Permissions.CanCreateChild)
                wall.AddRoute = urlHelper.Link<RoutesController>(c => c.Post(wall.Id, null));
            if (wall.Permissions.CanModify)
            {
                wall.UploadImage = urlHelper.Link<WallsController>(c => c.PatchWallImage(wall.Id));
                wall.Update = urlHelper.Link<WallsController>(c => c.Patch(wall.Id, null));
            }
            if (wall.Permissions.CanDelete)
                wall.Delete = urlHelper.Link<WallsController>(c => c.Delete(wall.Id));
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