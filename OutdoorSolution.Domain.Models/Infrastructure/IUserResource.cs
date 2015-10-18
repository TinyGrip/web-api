using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Domain.Models.Infrastructure
{
    /// <summary>
    /// A resource that is created by the user
    /// </summary>
    public interface IUserResource
    {
        /// <summary>
        /// An id of the user that created resource
        /// </summary>
        string UserId { get; set; }
    }
}
