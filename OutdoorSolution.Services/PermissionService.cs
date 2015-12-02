using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Interfaces;

namespace OutdoorSolution.Services
{
    /// <summary>
    /// Checkes if user has a permissions for resource
    /// </summary>
    public class PermissionService : IPermissionService
    {
        public PermissionService()
        {

        }

        //public bool CanUserModifyResource<T>(string userId, T resource) where T : IUserResource
        //{
            
        //    return (resource.UserId == user.Identity.GetUserId()) || user.IsInRole(RoleNames.Admin);
        //}

        //public bool CanUserDeleteResource<T>(string userId, T resource) where T : IUserResource
        //{
        //    return (resource.UserId == user.Identity.GetUserId()) || user.IsInRole(RoleNames.Admin);
        //}

        public bool CanUserAccessResource<T>(string userId, T resource, PermissionType permission) where T : IUserResource
        {
            //if (permission == PermissionType.Update)
            //{
            //    return CanUserModifyResource<T>(userId, resource);
            //}
            //else if (permission == PermissionType.Delete)
            //{
            //    return CanUserDeleteResource<T>(userId, resource);
            //}
            //else
                return true; // true for now
                
        }

        //public bool CanUserDeleteResource<T>(string userId, AreaImage resource) where T : AreaImage
        //{
        //    return (resource.Area.UserId == user.Identity.GetUserId()) || user.IsInRole(RoleNames.Admin);
        //}
    }
}
