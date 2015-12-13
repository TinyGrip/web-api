using System;
using System.Collections.Generic;
using System.Web.Http;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Reflection;

namespace OutdoorSolution.Controllers
{
    public class RootController : ApiController
    {
        private const string USER_NODES_ROUTE = "api/User";

        public IHttpActionResult Get()
        {
            var nodes = new Dictionary<string, object>();
            nodes.Add( "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            nodes.Add( "PreviewAreas", Url.Link<PreviewAreasController>(c => c.Get(null)) );
            nodes.Add( "User", Url.GetSpecialResource(USER_NODES_ROUTE) );

            return Ok(nodes);
        }

        [Route(USER_NODES_ROUTE)]
        public IHttpActionResult GetUserLink()
        {
            // add account data
            var accountNodes = new Dictionary<string, object>();
            
            // TODO: use commented, when link generation is improved 
            if (User.Identity.IsAuthenticated)
            {
                var userId = new Guid(User.Identity.GetUserId());
                accountNodes.Add("UserInfo", Url.Link<UserInfoController>(c => c.GetById(userId)));
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

            return Ok(accountNodes);
        }
    }
}
