using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    public class PermissionsData
    {
        public PermissionsData()
        {
            CanView = true;
        }

        public bool CanView { get; set; }

        public bool CanCreateChild { get; set; }

        public bool CanModify { get; set; }

        public bool CanDelete { get; set; }
    }
}
