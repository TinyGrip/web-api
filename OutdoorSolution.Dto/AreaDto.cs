using Newtonsoft.Json;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OutdoorSolution.Dto
{
    public class AreaDto : UserResourceDto
    {
        public AreaDto()
        {
            Images = new List<AreaImageDto>();
        }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(32768)]
        public string Description { get; set; }

        public DateTime? Created { get; set; }

        public double Rating { get; set; }

        public int RatingsCount { get; set; }

        public GeographyDto Location { get; set; }

        public IEnumerable<AreaImageDto> Images { get; set; }

        public IEnumerable<WallDto> PreviewWalls { get; set; }

        public IEnumerable<RouteDto> PreviewRoutes { get; set; }

        [JsonIgnore]
        public bool CanComment { get; set; }

        // --------------- Links section -----------------

        public Link Walls { get; set; }

        public Link Comments { get; set; }

        public Link AddWall { get; set; }

        public Link AddImage { get; set; }

        public Link AddComment { get; set; }
    }
}
