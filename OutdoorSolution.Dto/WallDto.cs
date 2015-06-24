using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class WallDto : PageItem
    {
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(512)]
        public string Image { get; set; }

        public GeographyDto Location { get; set; }

        public Link Routes { get; set; }
    }
}
