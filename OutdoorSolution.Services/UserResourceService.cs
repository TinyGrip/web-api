using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Exceptions;
using OutdoorSolution.Services.Interfaces;
using System;
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
        protected readonly IPermissionService permissionService;

        public UserResourceService(IUnitOfWork unitOfWork, IPermissionService permissionsService)
        {
            this.unitOfWork = unitOfWork;
            this.permissionService = permissionsService;
        }

        public string UserId { get; set; }

        protected async Task<T> GetResource(Guid id, PermissionType permissionType)
        {
            var resource = await unitOfWork.Set<T>().FindAsync(id);
            if (resource == null)
                throw new ObjectNotFoundException();

            if (!permissionService.CanUserAccessResource(UserId, resource, permissionType))
                throw new AccessDeniedException();

            return resource;
        }
    }
}
