using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using OutdoorSolution.Dto.Infrastructure;

namespace OutdoorSolution.Models
{
    /// <summary>
    /// Url parameters, used for pagination
    /// </summary>
    public class PagingParams : IPagingData
    {
        public PagingParams()
        {
            int itemsPerPage;
            // TODO: move to settings service
            if (int.TryParse(ConfigurationManager.AppSettings["ItemsPerPage"], out itemsPerPage))
                Take = itemsPerPage;
            else
                Take = 10;
        }

        public int Skip { get; set; }

        public int Take { get; set; }

        public int TotalAmount { get; set; }
    }
}
