using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using OutdoorSolution.Services.Results;
using OutdoorSolution.Services.Interfaces;

namespace OutdoorSolution.Services
{
    public class RouteService : UserResourceService<Route>, IRouteService
    {
        public RouteService(IUnitOfWork unitOfWork, IPermissionService permissionService):
            base(unitOfWork, permissionService)
        {
        }

        public async Task<RouteDto> GetById(Guid id)
        {
            var route = await GetResource(id, PermissionType.Read);
            return CreateRouteDto(route);
        }

        public async Task<List<RouteDto>> Get(Guid wallId, IPagingData pagingData)
        {
            var q = unitOfWork.Routes.Where(x => x.WallId == wallId);
            pagingData.TotalAmount = q.Count();
            if (pagingData.TotalAmount == 0)
            {
                return new List<RouteDto>();
            }

            var routes = await q.OrderByDescending(x => x.Name)
                                      .Skip(pagingData.Skip)
                                      .Take(pagingData.Take)
                                      .ToListAsync();

            var routesDto = routes.Select(x => CreateRouteDto(x)).ToList();

            return routesDto;
        }

        public ResourceWrapper<RouteDto> Create(Guid wallId, RouteDto routeDto)
        {
            var route = new Route();
            UpdateRoute(route, routeDto);
            route.UserId = UserId;
            route.WallId = wallId;
            unitOfWork.Routes.Add(route);

            return new ResourceWrapper<RouteDto>(() => RouteService.CreateRouteDto(route));
        }

        public async Task Update(Guid id, RouteDto routeDto)
        {
            var route = await GetResource(id, PermissionType.Update);
            UpdateRoute(route, routeDto);
        }

        public async Task Delete(Guid id)
        {
            var route = await GetResource(id, PermissionType.Delete);
            unitOfWork.Routes.Remove(route);
        }

        internal static RouteDto CreateRouteDto(Route route)
        {
            var routeDto = new RouteDto()
            {
                Id = route.Id,
                Name = route.Name,
                Type = route.Type,
                Complexity = route.Complexity,
                Path = Utils.ConvertDbGeometry(route.Path),
                WallId = route.WallId
            };

            return routeDto;
        }

        private void UpdateRoute(Route route, RouteDto routeDto)
        {
            route.Name = routeDto.Name;
            route.Type = routeDto.Type;
            route.Complexity = routeDto.Complexity;
            route.Path = Utils.CreateDbGeometry(routeDto.Path);
        }
    }
}
