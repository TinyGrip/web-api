using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IPermissionService : IService
    {
        bool CanUserAccessResource<T>(string userId, T resource, PermissionType permission) where T : IUserResource;
    }
}
