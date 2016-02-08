using OutdoorSolution.Domain.Models.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutdoorSolution.Domain.Models
{
    public class RouteComment : Comment
    {
        [Required]
        [ForeignKey("Route")]
        public override Guid SubjectId { get; set; }

        public virtual Route Route { get; set; }
    }
}
