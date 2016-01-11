using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        public string Grade { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RouteType Type { get; set; }

        [JsonIgnore]
        public Guid WallId { get; set; }

        public Link Wall { get; set; }
    }
}
