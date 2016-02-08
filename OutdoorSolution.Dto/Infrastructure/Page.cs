using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    public class Page<T> where T : class
    {
        public int TotalAmount { get; set; }

        public IEnumerable<T> Items { get; set; }

        public Link Next { get; set; }

        public Link Prev { get; set; }
    }
}
