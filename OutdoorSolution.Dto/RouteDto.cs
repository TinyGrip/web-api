using OutdoorSolution.Common;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class RouteDto : PageItem
    {
        public string Name { get; set; }

        public IEnumerable<PointDto> Path { get; set; }

        public double Complexity { get; set; }

        public RouteType Type { get; set; }

        public Link Wall { get; set; }
    }
}
