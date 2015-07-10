using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Dto.Infrastructure
{
    /// <summary>
    /// Having own enum for this, is only for thew sake of independence from MVC assemblies
    /// </summary>
    public enum HttpMethods 
    {
        Get,
        Post,
        Put,
        Delete
    }
}
