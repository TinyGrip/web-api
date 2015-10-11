using OutdoorSolution.Domain.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Domain.Models
{
    public class Area : PreviewArea
    {
        public Area()
        {
            Walls = new List<Wall>();
            Images = new List<AreaImage>();
        }

        [MaxLength(32768)]
        public string Description { get; set; }

        public DateTime Created { get; set; }

        public int RatingsCount { get; set; }

        public virtual ICollection<Wall> Walls { get; set; }

        public virtual ICollection<AreaImage> Images { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
