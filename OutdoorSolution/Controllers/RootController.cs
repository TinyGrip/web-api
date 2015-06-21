using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OutdoorSolution.Controllers
{
    public class RootController : ApiController
    {
        public IHttpActionResult Get()
        {
            var nodes = new Dictionary<string, object>();
            nodes.Add("Places", "/Places?{searches}");

            nodes.Add("Events", "/Events");

            // account
            var accountActions = new NameValueCollection();
            nodes.Add("Account", accountActions);

            return Ok(nodes);
        }
    }
}
