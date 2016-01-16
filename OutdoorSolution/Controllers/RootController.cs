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
        public IHttpActionResult Get()
        {
            var nodes = new Dictionary<string, object>();
            nodes.Add( "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            nodes.Add( "PreviewAreas", Url.Link<PreviewAreasController>(c => c.Get(null)) );
            nodes.Add( "AddArea", Url.Link<AreasController>(c => c.PostArea(null))) ;

            var authNodes = new Dictionary<string, object>();
            authNodes.Add( "User", Url.Link<UserInfoController>(c => c.Get(null)) );
            
            authNodes.Add( "Token", Url.GetSpecialResource(Startup.OAuthOptions.TokenEndpointPath.Value, "POST") );
            authNodes.Add( "Register", Url.Link<AccountController>(c => c.Register(null)) );

            nodes.Add( "Auth", authNodes );

            return Ok(nodes);
        }
    }
}
