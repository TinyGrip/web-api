using OutdoorSolution.Domain.Models.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutdoorSolution.Domain.Models
{
    public class AreaComment : Comment
    {
        [Required]
        [ForeignKey("Area")]
        public override Guid SubjectId { get; set; }

        public virtual Area Area { get; set; }
    }
}
