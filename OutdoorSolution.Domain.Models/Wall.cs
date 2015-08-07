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
    public class Wall
    {
        public Wall()
        {
            Routes = new List<Route>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(512)]
        public string Image { get; set; }

        public DbGeography Location { get; set; }

        public Guid AreaId { get; set; }

        public virtual ICollection<Route> Routes { get; set; }

        public virtual Area Area { get; set; }
    }
}
