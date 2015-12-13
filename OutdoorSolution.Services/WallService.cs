using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using OutdoorSolution.Services.Common;
using System.IO;
using OutdoorSolution.Services.Results;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Services.Exceptions;

namespace OutdoorSolution.Services
{
    public class WallService : UserResourceService<Wall>, IWallService
    {
        private readonly IFileSystemService fsService;

        public WallService(IUnitOfWork unitOfWork, IFileSystemService fsService, TGUserManager userManager) :
            base(unitOfWork, userManager)
        {
            this.fsService = fsService;
        }

        public async Task<WallDto> GetById(Guid id)
        {
            var wall = await GetResource(id, PermissionType.Read);
            return await CreateWallDto(wall);
        }

        public async Task<List<WallDto>> Get(Guid areaId, IPagingData pagingData)
        {
            var q = unitOfWork.Walls.Where(w => w.AreaId == areaId);
            pagingData.TotalAmount = q.Count();

            if (pagingData.TotalAmount == 0)
                return new List<WallDto>();

            var walls = await q.OrderByDescending(a => a.Name)
                               .Skip(pagingData.Skip)
                               .Take(pagingData.Take)
                               .ToListAsync();

            var wallDtos = await Utils.WhenAllSeq(walls.Select(x => CreateWallDto(x)));
            return wallDtos;
        }

        public ResourceWrapper<WallDto> Create(Guid areaId, WallDto wallDto)
        {
            if (UserId == null)
                throw new UserIsNullException();

            var wall = new Wall();
            UpdateWall(wall, wallDto);
            wall.AreaId = areaId;
            wall.UserId = UserId;
            
            unitOfWork.Walls.Add(wall);

            return new ResourceWrapper<WallDto>( () => CreateWallDto(wall) );
        }

        public async Task Update(Guid id, WallDto wallDto)
        {
            var wall = await GetResource(id, PermissionType.Update);
            fsService.DeleteImage(wall.Image);
            UpdateWall(wall, wallDto);
        }

        /// <summary>
        /// Updates wall image file
        /// </summary>
        /// <param name="wallId"></param>
        /// <param name="imageStream"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public async Task UpdateImage(Guid wallId, Stream imageStream, string fileExtension)
        {
            var wall = await GetResource(wallId, PermissionType.Update);

            // delete old wall image file if needed
            fsService.DeleteImage(wall.Image);

            // save new image to file
            var relImagePath = fsService.SaveImageStreamToFile(imageStream, fileExtension);
            wall.Image = relImagePath;
        }

        public async Task Delete(Guid id)
        {
            var wall = await GetResource(id, PermissionType.Delete);

            // remove wall image file
            fsService.DeleteImage(wall.Image);

            unitOfWork.Walls.Remove(wall);
        }

        private async Task<WallDto> CreateWallDto(Wall wall)
        {
            var wallDto = new WallDto()
            {
                Id = wall.Id,
                Name = wall.Name,
                AreaId = wall.AreaId,
                ImageHref = wall.Image,
                Location = Utils.CreateGeoDto(wall.Location)
            };

            await SetPermissions(wallDto, wall);

            return wallDto;
        }

        private void UpdateWall(Wall wall, WallDto wallDto)
        {
            wall.Name = wallDto.Name;
            wall.Image = wallDto.ImageHref;
            wall.Location = Utils.CreateDbPoint(wallDto.Location);
        }
    }
}
