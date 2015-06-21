using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Domain.Models
{
    public class RouteComment : BaseComment
    {
        public Guid RouteId { get; set; }

        public Route Route { get; set; }
    }
}
