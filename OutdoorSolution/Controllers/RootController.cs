using System;
using System.Collections.Generic;
using System.Web.Http;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using Microsoft.AspNet.Identity;

namespace OutdoorSolution.Controllers
{
    public class RootController : ApiController
    {
        public IHttpActionResult Get()
        {
            var nodes = new Dictionary<string, object>();
            nodes.Add( "PreviewAreas", Url.Link<PreviewAreasController>(c => c.Get(null)) );

            // add account data
            var accountNodes = new Dictionary<string, object>();
            
            // TODO: use commented, when link generation is improved 
            if (User.Identity.IsAuthenticated)
            {
                var userId = new Guid(User.Identity.GetUserId());
                nodes.Add( "UserInfo", Url.Link<UserInfoController>(c => c.GetById(userId)) );
                
                accountNodes.Add("ChangePassword", Url.GetSpecialResource("/api/Account/ChangePassword"));
                //accountNodes.Add( "Logout", Url.Link<AccountController>(c => c.Logout()) );
                accountNodes.Add("Logout", Url.GetSpecialResource("/api/Account/Logout"));
            }
            else
            {
                accountNodes.Add("Login", Url.GetSpecialResource("/Token", HttpMethods.Post));
                //accountNodes.Add( "Register", Url.Link<AccountController>(c => c.Register(null)) );
                accountNodes.Add("Register", Url.GetSpecialResource("/api/Account/Register", HttpMethods.Post));
            }

            nodes.Add( "Account", accountNodes );

            return Ok(nodes);
        }
    }
}
