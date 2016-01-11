using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OutdoorSolution.Common;
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

        private FreeClimbingGradesSystems freeClimbingGradesSystem;
        public FreeClimbingGradesSystems FreeClimbingGradesSystem
        {
            get { return freeClimbingGradesSystem; }
            set
            {
                if (!Enum.IsDefined(typeof(FreeClimbingGradesSystems), value))
                    throw new ArgumentException("Wrong free climbing grade value");
                freeClimbingGradesSystem = value;
            }
        }

        private BoulderingGradesSystems boulderingGradesSystem;
        public BoulderingGradesSystems BoulderingGradesSystem 
        {
            get { return boulderingGradesSystem; }
            set
            {
                if (!Enum.IsDefined(typeof(BoulderingGradesSystems), value))
                    throw new ArgumentException("Wrong bouldering grade value");

                boulderingGradesSystem = value;
            }
        }

        public string[] FreeClimbingGrades { get; set; }

        public string[] BoulderingGrades { get; set; }

        // ---- Links ----

        public Link Self { get; set; }

        public Link Update { get; set; }

        public Link UploadAvatarImage { get; set; }

        public Link UploadCoverImage { get; set; }

        public Link AvatarImage { get; set; }

        public Link CoverImage { get; set; }

        public Link ChangePassword { get; set; }

        public Link Logout { get; set; }
    }
}
