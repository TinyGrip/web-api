using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OutdoorSolution.Domain.Models;

namespace OutdoorSolution.Services
{
    /// <summary>
    /// Checkes if user has a permissions for resource
    /// </summary>
    public class PermissionsService
    {
        public bool CanUserModifyResource<T>(IPrincipal user, T resource) where T : IUserResource
        {
            return (resource.UserId == user.Identity.GetUserId()) || user.IsInRole(RoleNames.Admin);
        }

        public bool CanUserDeleteResource<T>(IPrincipal user, T resource) where T : IUserResource
        {
            return (resource.UserId == user.Identity.GetUserId()) || user.IsInRole(RoleNames.Admin);
        }

        public bool CanUserDeleteResource(IPrincipal user, AreaImage resource)
        {
            return (resource.Area.UserId == user.Identity.GetUserId()) || user.IsInRole(RoleNames.Admin);
        }
    }
}
