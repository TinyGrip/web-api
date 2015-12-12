using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Exceptions;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutdoorSolution.Services
{
    /// <summary>
    /// Base service for resources, created by user
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UserResourceService<T> where T : class, IUserResource
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly TGUserManager userManager;

        public UserResourceService(IUnitOfWork unitOfWork, TGUserManager userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public virtual string UserId { get; set; }

        public async Task<bool> CanUserAccessResource(Guid resourceId, PermissionType permission)
        {
            var resource = await unitOfWork.Set<T>().FindAsync(resourceId);
            if (resource == null)
                throw new ObjectNotFoundException();

            return await CanUserAccessResource(resource, permission);
        }

        protected async Task<bool> CanUserAccessResource(T resource, PermissionType permission)
        {
            if (permission == PermissionType.Update || permission == PermissionType.Delete)
            {
                if (String.IsNullOrEmpty(UserId))
                    return false;
                else
                    return resource.UserId == UserId || await userManager.IsInRoleAsync(UserId, RoleNames.Admin);
            }

            if (permission == PermissionType.Create)
            {
                return !String.IsNullOrEmpty(UserId);
            }

            return true;
        }

        protected async Task<T> GetResource(Guid id, PermissionType permissionType)
        {
            var resource = await unitOfWork.Set<T>().FindAsync(id);
            if (resource == null)
                throw new ObjectNotFoundException();

            var userHasPermission = await CanUserAccessResource(resource, permissionType);
            if (!userHasPermission)
                throw new AccessDeniedException();

            return resource;
        }

        protected async Task SetPermissions(UserResourceDto dto, T resource)
        {
            dto.Permissions.CanCreateChild = await CanUserAccessResource(resource, PermissionType.Create);
            dto.Permissions.CanModify = await CanUserAccessResource(resource, PermissionType.Update);
            dto.Permissions.CanDelete = await CanUserAccessResource(resource, PermissionType.Update);
        }
    }
}
