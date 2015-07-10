using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    public class Link
    {
        public Uri Href { get; set; }

        public bool Templated { get; set; }

        /// <summary>
        /// Http method for using this Link's Href. Null value implies default method - GET
        /// </summary>
        public HttpMethods? Method { get; set; }
    }
}
