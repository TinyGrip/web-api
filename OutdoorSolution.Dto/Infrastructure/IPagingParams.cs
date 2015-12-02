using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    /// <summary>
    /// Pagination data
    /// </summary>
    public interface IPagingData
    {
        int Skip { get; set; }

        int Take { get; set; }

        int TotalAmount { get; set; }
    }
}
