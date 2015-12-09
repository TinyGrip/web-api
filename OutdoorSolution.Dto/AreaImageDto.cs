using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class AreaImageDto : UserResourceDto
    {
        public string Name { get; set; }

        public string Href { get; set; }

        public Link Link { get; set; }
    }
}
