using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Common
{
    [JsonConverter(typeof(OnlyStringEnumConverter))]
    public enum RouteType
    {
        [EnumMember(Value = "Boulder")]
        Boulder,
        [EnumMember(Value = "Lead")]
        Lead,
        [EnumMember(Value = "Rope")]
        Rope
    }

    [JsonConverter(typeof(OnlyStringEnumConverter))]
    public enum FreeClimbingGradesSystems
    {
        [EnumMember(Value = "French")]
        French,
        [EnumMember(Value = "YDS")]
        YDS
    }

    [JsonConverter(typeof(OnlyStringEnumConverter))]
    public enum BoulderingGradesSystems
    {
        [EnumMember(Value = "French")]
        French,
        [EnumMember(Value = "Hueco")]
        Hueco
    }
}
