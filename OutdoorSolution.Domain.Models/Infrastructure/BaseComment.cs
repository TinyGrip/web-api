using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Domain.Models.Infrastructure
{
    public abstract class BaseComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [DataType(DataType.Text)]
        public string Text { get; set; }

        public DateTime Created { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
