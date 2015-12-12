using Newtonsoft.Json;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto
{
    public class UserInfoDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string AvatarHref { get; set; }

        public string CoverHref { get; set; }

        // ---- Links ----

        public Link Self { get; set; }

        public Link UploadAvatarImage { get; set; }

        public Link UploadCoverImage { get; set; }

        public Link AvatarImage { get; set; }

        public Link CoverImage { get; set; }
    }
}
