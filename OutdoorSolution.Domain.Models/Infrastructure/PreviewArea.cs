using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Domain.Models.Infrastructure
{
    public class PreviewArea
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public double Rating { get; set; }

        public DbGeography Location { get; set; }
    }
}
