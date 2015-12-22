using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        public string Method { get; set; }
    }
}
