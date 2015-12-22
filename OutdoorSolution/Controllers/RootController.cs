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
            nodes.Add( "User", Url.Link<RootController>(c => c.GetUserLink()) );

            return Ok(nodes);
        }

        [Route(USER_NODES_ROUTE)]
        public IHttpActionResult GetUserLink()
        {
            // add account data
            var accountNodes = new Dictionary<string, object>();
            
            if (User.Identity.IsAuthenticated)
            {
                var userId = new Guid(User.Identity.GetUserId());
                accountNodes.Add( "UserInfo", Url.Link<UserInfoController>(c => c.GetById(userId)) );
                accountNodes.Add( "ChangePassword", Url.Link<AccountController>(c => c.ChangePassword(null)) );
                accountNodes.Add( "Logout", Url.Link<AccountController>(c => c.Logout()) );
            }
            else
            {
                accountNodes.Add( "Login", Url.GetSpecialResource("Token", "POST") );
                accountNodes.Add( "Register", Url.Link<AccountController>(c => c.Register(null)) );
            }

            return Ok(accountNodes);
        }
    }
}
