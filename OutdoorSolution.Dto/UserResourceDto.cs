using Newtonsoft.Json;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public abstract class UserResourceDto : PageItem
    {
        public UserResourceDto()
        {
            Permissions = new PermissionsData();
        }

        [JsonIgnore]
        public PermissionsData Permissions { get; set; }
    }
}
