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
using OutdoorSolution.Services.Exceptions;

namespace OutdoorSolution.Services
{
    public class RouteService : UserResourceService<Route>, IRouteService
    {
        public RouteService(IUnitOfWork unitOfWork, TGUserManager userManager) :
            base(unitOfWork, userManager)
        {
        }

        public async Task<RouteDto> GetById(Guid id)
        {
            var route = await GetResource(id, PermissionType.Read);
            return await CreateRouteDto(route);
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

            var routesDto = await Utils.WhenAllSeq(routes.Select(x => CreateRouteDto(x)));
            return routesDto;
        }

        public async Task<List<RouteDto>> GetByArea(Guid areaId, IPagingData pagingData)
        {
            // TODO: debug this db request
            var q = unitOfWork.Routes.Where(x => x.Wall.AreaId == areaId);
            pagingData.TotalAmount = q.Count();
            if (pagingData.TotalAmount == 0)
            {
                return new List<RouteDto>();
            }

            var routes = await q.OrderByDescending(x => x.Name)
                                .Skip(pagingData.Skip)
                                .Take(pagingData.Take)
                                .ToListAsync();

            var routesDto = await Utils.WhenAllSeq(routes.Select(x => CreateRouteDto(x)));

            return routesDto;
        }

        public ResourceWrapper<RouteDto> Create(Guid wallId, RouteDto routeDto)
        {
            if (UserId == null)
                throw new UserIsNullException();

            var route = new Route();
            UpdateRoute(route, routeDto);
            route.UserId = UserId;
            route.WallId = wallId;
            unitOfWork.Routes.Add(route);

            return new ResourceWrapper<RouteDto>( () => CreateRouteDto(route) );
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

        private async Task<RouteDto> CreateRouteDto(Route route)
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

            await SetPermissions(routeDto, route);

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
