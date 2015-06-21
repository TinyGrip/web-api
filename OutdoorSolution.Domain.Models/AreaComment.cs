using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Domain.Models
{
    public class AreaComment : BaseComment
    {
        public Guid AreaId { get; set; }

        public virtual Area Area { get; set; }
    }
}
