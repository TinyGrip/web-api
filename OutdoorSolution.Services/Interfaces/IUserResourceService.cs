using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    /// <summary>
    /// Base interface for all services, that are working with user-created resources
    /// </summary>
    public interface IUserResourceService
    {
        string UserId { get; set; }
    }
}
