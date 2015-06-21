using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class PagingParams
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public int TotalAmount { get; set; }
    }
}
