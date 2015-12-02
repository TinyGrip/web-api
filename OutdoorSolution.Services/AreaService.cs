using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Services.Results;
using OutdoorSolution.Services.Interfaces;

namespace OutdoorSolution.Services
{
    public class AreaService : UserResourceService<Area>, IAreaService
    {
        private const int PREVIEW_ITEMS_COUNT = 3;
        IWallService wallService;

        public AreaService(IUnitOfWork unitOfWork, IPermissionService permissionsService, IWallService wallService)
            : base(unitOfWork, permissionsService)
        {
            this.wallService = wallService;
        }

        public async Task<AreaDto> GetById(Guid id)
        {
            var area = await GetResource(id, PermissionType.Read);
            return CreateDto(area);
        }

        public async Task<IEnumerable<AreaDto>> Get(IPagingData pagingData)
        {
            pagingData.TotalAmount = unitOfWork.Areas.Count();
            if (pagingData.TotalAmount == 0)
            {
                return new AreaDto[0];
            }

            var areas = await unitOfWork.Areas.OrderByDescending(a => a.Name)
                                      .Skip(pagingData.Skip)
                                      .Take(pagingData.Take)
                                      .ToListAsync();

            // TODO: think about memory
            var areaDtos = areas.Select(a => CreateDto(a)).ToList();

            return areaDtos;
        }

        public async Task<IEnumerable<AreaDto>> GetPreview(IPagingData pagingData)
        {
            pagingData.TotalAmount = unitOfWork.Areas.Count();
            // create lighter request, by using preview model
            // TODO: check if this has actually better performance
            var areas = await unitOfWork.Areas.OrderByDescending(a => a.Name)
                                      .Skip(pagingData.Skip)
                                      .Take(pagingData.Take)
                                      .Select(a => new PreviewArea()
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Rating = a.Rating,
                                          Location = a.Location
                                      })
                                      .ToListAsync();

            var areaDtos = areas.Select(a => CreatePreviewAreaDto(a)).ToList();
            return areaDtos;
        }

        public ResourceWrapper<AreaDto> Create(AreaDto areaDto)
        {
            var area = new Area();

            // create db model and save it
            UpdateArea(area, areaDto);
            area.Created = DateTime.UtcNow;
            if (areaDto.Images != null)
                area.Images = areaDto.Images.Select(x => AreaImageService.CreateAreaImage(x)).ToList();

            unitOfWork.Areas.Add(area);

            return new ResourceWrapper<AreaDto>(() => AreaService.CreateDto(area));
        }

        public async Task Update(Guid id, AreaDto areaDto)
        {
            var area = await GetResource(id, PermissionType.Update);
            UpdateArea(area, areaDto);
        }

        public async Task Delete(Guid id)
        {
            var area = await GetResource(id, PermissionType.Delete);
            
            // delete wall though wall service, because it untilizes resources from outside of DB
            for (int i = area.Walls.Count - 1; i >= 0; --i)
            {
                // TODO: check performance vs wallService.Delete(wall)
                await wallService.Delete(area.Walls.ElementAt(i).Id);
            }
            
            unitOfWork.Areas.Remove(area);
        }

        internal static AreaDto CreateDto(Area area)
        {
            var areaDto = new AreaDto()
            {
                Name = area.Name,
                Description = area.Description,
                Created = area.Created,
                Rating = area.Rating,
                RatingsCount = area.RatingsCount,
                Images = area.Images.Select(x => AreaImageService.CreateAreaImageDto(x)).ToList()
            };

            areaDto.Location = Utils.CreateGeoDto(area.Location);

            // add previews
            areaDto.PreviewWalls = area.Walls.OrderBy(x => x.Name)
                                             .Take(PREVIEW_ITEMS_COUNT)
                                             .ToList()
                                             .Select(w => WallService.CreateWallDto(w))
                                             .ToList();

            // TODO: debug requests to DB
            areaDto.PreviewRoutes = area.Walls.SelectMany(w => w.Routes)
                                              .OrderBy(r => r.Complexity)
                                              .Take(PREVIEW_ITEMS_COUNT)
                                              .ToList()
                                              .Select(r => RouteService.CreateRouteDto(r))
                                              .ToList();

            return areaDto;
        }

        internal static AreaDto CreatePreviewAreaDto(PreviewArea area)
        {
            var areaDto = new AreaDto()
            {
                Name = area.Name,
                Rating = area.Rating
            };

            areaDto.Location = Utils.CreateGeoDto(area.Location);
            return areaDto;
        }

        private void UpdateArea(Area area, AreaDto areaDto)
        {
            area.Name = areaDto.Name;
            area.Location = Utils.CreateDbPoint(areaDto.Location);
            area.Description = areaDto.Description;
        }
    }
}
