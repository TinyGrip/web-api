using OutdoorSolution.Common;
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
    public class Route: IUserResource
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public DbGeometry Path { get; set; }

        public double Complexity { get; set; }

        private RouteType type { get; set; }
        public RouteType Type
        {
            get { return type; }
            set
            {
                if (!Enum.IsDefined(typeof(RouteType), value))
                    throw new ArgumentException("Wrong route type value");
                type = value;  
            }
        }

        public Guid WallId { get; set; }

        public virtual Wall Wall { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
