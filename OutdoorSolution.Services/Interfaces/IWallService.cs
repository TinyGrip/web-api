using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IWallService : IService
    {
        string UserId { get; set; }

        Task<WallDto> GetById(Guid id);

        Task<IEnumerable<WallDto>> Get(Guid areaId, IPagingData pagingData);

        ResourceWrapper<WallDto> Create(Guid areaId, WallDto wallDto);

        Task Update(Guid id, WallDto wallDto);

        Task UpdateImage(Guid wallId, Stream imageStream, string fileExtension);

        Task Delete(Guid id);
    }
}
