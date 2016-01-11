using OutdoorSolution.Controllers;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Helpers;
using OutdoorSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace OutdoorSolution.Links
{
    /// <summary>
    /// Creates hypermedia links on user info dto object
    /// </summary>
    public class UserInfoLinker : ILinker
    {
        public void Linkify(UserInfoDto userInfo, UrlHelper urlHelper)
        {
            if (!String.IsNullOrEmpty(userInfo.AvatarHref))
            {
                userInfo.AvatarImage = new Link()
                {
                    Href = ImageHelper.GetImageUri(userInfo.AvatarHref, urlHelper.Request.RequestUri)
                };
            }

            if (!String.IsNullOrEmpty(userInfo.CoverHref))
            {
                userInfo.CoverImage = new Link()
                {
                    Href = ImageHelper.GetImageUri(userInfo.CoverHref, urlHelper.Request.RequestUri)
                };
            }

            userInfo.AvatarHref = null;
            userInfo.CoverHref = null;

            userInfo.Update = urlHelper.Link<UserInfoController>(c => c.Put(userInfo.Id, null));

            userInfo.UploadAvatarImage = urlHelper.Link<UserInfoController>(c => c.PatchImage(userInfo.Id, UserImageTypes.Avatar));
            userInfo.UploadCoverImage = urlHelper.Link<UserInfoController>(c => c.PatchImage(userInfo.Id, UserImageTypes.Cover));

            userInfo.ChangePassword = urlHelper.Link<AccountController>(c => c.ChangePassword(null));
            userInfo.Logout = urlHelper.Link<AccountController>(c => c.Logout());

            userInfo.Self = urlHelper.Link<UserInfoController>(c => c.GetById(userInfo.Id));
        }
    }
}