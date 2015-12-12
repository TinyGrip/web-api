using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IAreaImageService : IUserResourceService, IService
    {
        Task<AreaImageDto> GetById(Guid id);

        Task<IEnumerable<AreaImageDto>> GetByArea(Guid areaId);

        ResourceWrapper<AreaImageDto> Create(Guid areaId, AreaImageDto areaImageDto);

        /// <summary>
        /// Creates image file in file system and updates area image url
        /// </summary>
        /// <param name="areaImageId"></param>
        /// <param name="imageStream"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        Task UpdateImage(Guid areaImageId, Stream imageStream, string fileExtension);

        Task Delete(Guid id);

        Task<bool> CanUserAccessResource(Guid resourceId, PermissionType permission);
    }
}
