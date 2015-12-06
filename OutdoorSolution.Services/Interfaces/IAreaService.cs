using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IAreaService : IService
    {
        string UserId { get; set; }

        Task<AreaDto> GetById(Guid id);

        Task<List<AreaDto>> Get(IPagingData pagingData);

        Task<List<AreaDto>> GetPreview(IPagingData pagingData);

        ResourceWrapper<AreaDto> Create(AreaDto areaDto);

        Task Update(Guid id, AreaDto areaDto);

        Task Delete(Guid id);
    }
}
