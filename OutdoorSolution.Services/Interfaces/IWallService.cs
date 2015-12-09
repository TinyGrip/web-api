using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IWallService : IUserResourceService, IService
    {
        Task<WallDto> GetById(Guid id);

        Task<List<WallDto>> Get(Guid areaId, IPagingData pagingData);

        ResourceWrapper<WallDto> Create(Guid areaId, WallDto wallDto);

        Task Update(Guid id, WallDto wallDto);

        Task UpdateImage(Guid wallId, Stream imageStream, string fileExtension);

        Task Delete(Guid id);

        Task<bool> CanUserAccessResource(Guid resourceId, PermissionType permission);
    }
}
