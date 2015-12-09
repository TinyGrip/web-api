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
        IWallService wallService;
        IRouteService routeService;
        private const int PREVIEW_ITEMS_COUNT = 3;

        public AreaService(IUnitOfWork unitOfWork, TGUserManager userManager, IWallService wallService, IRouteService routeService)
            : base(unitOfWork, userManager)
        {
            this.wallService = wallService;
            this.routeService = routeService;
        }

        public async Task<AreaDto> GetById(Guid id)
        {
            var area = await GetResource(id, PermissionType.Read);
            return await CreateDto(area);
        }

        public async Task<List<AreaDto>> Get(IPagingData pagingData)
        {
            pagingData.TotalAmount = unitOfWork.Areas.Count();
            if (pagingData.TotalAmount == 0)
            {
                return new List<AreaDto>();
            }

            var areas = await unitOfWork.Areas.OrderByDescending(a => a.Name)
                                      .Skip(pagingData.Skip)
                                      .Take(pagingData.Take)
                                      .ToListAsync();

            // TODO: think about memory
            var areaDtos = await Utils.WhenAllSeq(areas.Select(a => CreateDto(a)));
            return areaDtos;
        }

        public async Task<List<AreaDto>> GetPreview(IPagingData pagingData)
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
            if (UserId == null)
                throw new UserIsNullException();

            var area = new Area();

            // create db model and save it
            UpdateArea(area, areaDto);
            area.Created = DateTime.UtcNow;
            area.UserId = UserId;
            if (areaDto.Images != null)
                area.Images = areaDto.Images.Select(x => AreaImageService.CreateAreaImage(x)).ToList(); // TODO: check user ID

            unitOfWork.Areas.Add(area);

            return new ResourceWrapper<AreaDto>(() => CreateDto(area));
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

        private async Task<AreaDto> CreateDto(Area area)
        {
            var areaDto = new AreaDto()
            {
                Id = area.Id,
                Name = area.Name,
                Description = area.Description,
                Created = area.Created,
                Rating = area.Rating,
                RatingsCount = area.RatingsCount,
                // get area images from service also
                Images = area.Images.Select(x => AreaImageService.CreateAreaImageDto(x)).ToList()
            };

            await SetPermissions(areaDto, area);
            areaDto.Location = Utils.CreateGeoDto(area.Location);

            // add previews
            var pagingData = new PagingData()
            {
                Take = PREVIEW_ITEMS_COUNT
            };
            areaDto.PreviewWalls = await wallService.Get(area.Id, pagingData);
            areaDto.PreviewRoutes = await routeService.GetByArea(area.Id, pagingData);

            return areaDto;
        }

        private static AreaDto CreatePreviewAreaDto(PreviewArea area)
        {
            var areaDto = new AreaDto()
            {
                Id = area.Id,
                Name = area.Name,
                Rating = area.Rating,

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
