using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Common
{
    public class PagingData : IPagingData
    {
        public int Skip
        {
            get;
            set;
        }

        public int Take
        {
            get;
            set;
        }

        public int TotalAmount
        {
            get;
            set;
        }
    }
}
