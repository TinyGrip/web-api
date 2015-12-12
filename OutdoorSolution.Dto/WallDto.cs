using Newtonsoft.Json;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace OutdoorSolution.Dto
{
    public class WallDto : UserResourceDto
    {
        [MaxLength(128)]
        public string Name { get; set; }

        public string ImageHref { get; set; }

        public GeographyDto Location { get; set; }

        [JsonIgnore]
        public Guid AreaId { get; set; }

        // ----- Links sections ------

        public Link Image { get; set; }

        public Link Area { get; set; }

        public Link Routes { get; set; }

        public Link AddRoute { get; set; }

        public Link UploadImage { get; set; }
    }
}
