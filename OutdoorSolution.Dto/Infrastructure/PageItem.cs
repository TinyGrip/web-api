using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    public class PageItem
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public Link Self { get; set; }

        public Link Update { get; set; }

        public Link Delete { get; set; }
    }
}
