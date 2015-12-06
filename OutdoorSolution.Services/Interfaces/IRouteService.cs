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
    public interface IRouteService : IService
    {
        string UserId { get; set; }

        Task<RouteDto> GetById(Guid id);

        Task<List<RouteDto>> Get(Guid wallId, IPagingData pagingData);

        ResourceWrapper<RouteDto> Create(Guid wallId, RouteDto routeDto);

        Task Update(Guid routeId, RouteDto routeDto);

        Task Delete(Guid id);
    }
}
