using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IAreaImageService : IUserResourceService, IService
    {
        Task<AreaImageDto> GetById(Guid id);

        Task<IEnumerable<AreaImageDto>> GetByArea(Guid areaId);

        ResourceWrapper<AreaImageDto> Create(Guid areaId, AreaImageDto areaImageDto);

        Task Delete(Guid id);

        Task<bool> CanUserAccessResource(Guid resourceId, PermissionType permission);
    }
}
