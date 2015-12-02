using OutdoorSolution.Dto;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IAreaImageService : IService
    {
        string UserId { get; set; }

        Task<AreaImageDto> GetById(Guid id);

        Task<IEnumerable<AreaImageDto>> GetByArea(Guid areaId);

        ResourceWrapper<AreaImageDto> Create(Guid areaId, AreaImageDto areaImageDto);

        Task Delete(Guid id);
    }
}
