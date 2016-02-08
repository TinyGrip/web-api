using Newtonsoft.Json;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class CommentDto : PageItem
    {
        public string Text { get; set; }

        public DateTime Created { get; set; }

        [JsonIgnore]
        public PermissionsData Permissions { get; set; }

        public Link Delete { get; set; }
    }
}
