using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using OutdoorSolution.Helpers;
using OutdoorSolution.Controllers;

namespace OutdoorSolution.Links
{
    /// <summary>
    /// Creates hypermedia links on area image dto object
    /// </summary>
    public class AreaImageLinker : ILinker
    {
        public void Linkify(AreaImageDto areaImage, UrlHelper urlHelper)
        {
            if (String.IsNullOrEmpty(areaImage.Href))
            {
                if (areaImage.Permissions.CanModify)
                    areaImage.UploadFile = urlHelper.Link<AreaImagesController>(c => c.PatchAreaImage(areaImage.Id));
            }
            else
            {
                areaImage.Link = ImageHelper.GetImageLink(areaImage.Href, urlHelper.Request.RequestUri);
                areaImage.Href = null;
            }

            if (areaImage.Permissions.CanDelete)
                areaImage.Delete = urlHelper.Link<AreaImagesController>(c => c.DeleteAreaImage(areaImage.Id));
        }

        public void Linkify(IEnumerable<AreaImageDto> areaImages, UrlHelper urlHelper)
        {
            foreach (var areaImage in areaImages)
            {
                Linkify(areaImage, urlHelper);
            }
        }
    }
}