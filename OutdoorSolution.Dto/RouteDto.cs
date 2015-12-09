using Newtonsoft.Json;
using OutdoorSolution.Common;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;

namespace OutdoorSolution.Dto
{
    public class RouteDto : UserResourceDto
    {
        public string Name { get; set; }

        public IEnumerable<PointDto> Path { get; set; }

        public double Complexity { get; set; }

        public RouteType Type { get; set; }

        [JsonIgnore]
        public Guid WallId { get; set; }

        public Link Wall { get; set; }
    }
}
