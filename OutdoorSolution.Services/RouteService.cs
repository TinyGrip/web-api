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
using OutdoorSolution.Common;

namespace OutdoorSolution.Services
{
    public class RouteService : UserResourceService<Route>, IRouteService
    {
        IRouteGradeService routeGradeService;

        public RouteService(IUnitOfWork unitOfWork, TGUserManager userManager, IRouteGradeService routeGradeService) :
            base(unitOfWork, userManager)
        {
            this.routeGradeService = routeGradeService;
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
                Path = Utils.ConvertDbGeometry(route.Path),
                WallId = route.WallId
            };
            
            // defaults
            var boulderingGradesSystem = BoulderingGradesSystems.French;
            var freeClimbingGradesSystem = FreeClimbingGradesSystems.French;
            
            // get user specific settings
            if (UserId != null)
            {
                var user = await userManager.FindByIdAsync(UserId);
                boulderingGradesSystem = user.BoulderingGradesSystem;
                freeClimbingGradesSystem = user.FreeClimbingGradesSystem;
            }

            if (route.Type == RouteType.Boulder)
            {
                routeDto.Grade = routeGradeService.ComplexityToGrade(route.Complexity, boulderingGradesSystem);
            }
            else
            {
                routeDto.Grade = routeGradeService.ComplexityToGrade(route.Complexity, freeClimbingGradesSystem);
            }

            await SetPermissions(routeDto, route);
            // allow to comment to any existing user
            var currUser = await userManager.FindByIdAsync(UserId);
            routeDto.CanComment = currUser != null;
            
            return routeDto;
        }

        private void UpdateRoute(Route route, RouteDto routeDto)
        {
            if (routeDto.Name != null)
                route.Name = routeDto.Name;
            if (routeDto.Type.HasValue)
                route.Type = routeDto.Type.Value;

            if (routeDto.Grade != null)
            {
                ApplicationUser user = userManager.Users.Single(u => u.Id == UserId); // TODO: think about async operation

                if (route.Type == RouteType.Boulder)
                {
                    route.Complexity = routeGradeService.GradeToComplexity(routeDto.Grade, user.BoulderingGradesSystem);
                }
                else
                {
                    route.Complexity = routeGradeService.GradeToComplexity(routeDto.Grade, user.FreeClimbingGradesSystem);
                }
            }

            if (routeDto.Path != null)
                route.Path = Utils.CreateDbGeometry(routeDto.Path);
        }
    }
}
