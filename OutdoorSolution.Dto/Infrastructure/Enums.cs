using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    /// <summary>
    /// Having own enum for this, is only for thew sake of independence from MVC assemblies
    /// </summary>
    public enum HttpMethods 
    {
        [EnumMember(Value="GET")]
        Get,
        [EnumMember(Value = "POST")]
        Post,
        [EnumMember(Value = "PUT")]
        Put,
        [EnumMember(Value = "DELETE")]
        Delete
    }
}
