using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class AreaDto : PageItem
    {
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(32768)]
        public string Description { get; set; }

        public DateTime Created { get; set; }

        public double Rating { get; set; }

        public int RatingsCount { get; set; }

        public GeographyDto Location { get; set; }

        public Link Walls { get; set; }

        public IEnumerable<AreaImageDto> Images { get; set; }
    }
}
