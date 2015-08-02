using OutdoorSolution.Common;
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
    public class Route
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public DbGeometry Path { get; set; }

        public double Complexity { get; set; }

        public RouteType Type { get; set; }

        public Guid WallId { get; set; }

        public virtual Wall Wall { get; set; }
    }
}
