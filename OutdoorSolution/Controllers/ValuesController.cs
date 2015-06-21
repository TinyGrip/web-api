using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OutdoorSolution.Helpers;

namespace OutdoorSolution.Controllers
{
    public class AB {
        public int Prop1 { get; set; }

        public string Prop2 { get; set; }
    }

    public class AC { public bool Prop3 { get; set; } }

    public class ValuesController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            var x = Url.Link<ValuesController>(a =>
                a.GetBy(DateTime.Now, new AB() { Prop1 = 10, Prop2 = "lalala" })
            );
            return Ok(x);
        }

        public string GetBy(DateTime t, [FromUri]AB t1)
        {
            return "PTN PTH";
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
