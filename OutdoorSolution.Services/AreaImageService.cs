using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Services.Exceptions;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using OutdoorSolution.Services.Results;
using System.IO;
using OutdoorSolution.Services.Common;

namespace OutdoorSolution.Services
{
    public class AreaImageService : UserResourceService<AreaImage>, IAreaImageService
    {
        IFileSystemService fsService;
        
        public AreaImageService(IUnitOfWork unitOfWork, TGUserManager userManager, IFileSystemService fsService)
            : base(unitOfWork, userManager)
        {
            this.fsService = fsService;
        }

        public async Task<AreaImageDto> GetById(Guid id)
        {
            var areaImage = await GetResource(id, Common.PermissionType.Read);
            var areaImageDto = await CreateAreaImageDto(areaImage);
            return areaImageDto;
        }

        public async Task<IEnumerable<AreaImageDto>> GetByArea(Guid areaId)
        {
            var areaImages = await unitOfWork.AreaImages.Where(ai => ai.AreaId == areaId).ToListAsync();
            var areaImagesDtos = await Utils.WhenAllSeq(areaImages.Select(ai => CreateAreaImageDto(ai)));
            return areaImagesDtos;
        }

        public ResourceWrapper<AreaImageDto> Create(Guid areaId, AreaImageDto areaImageDto)
        {
            if (UserId == null)
                throw new UserIsNullException();

            var areaImage = CreateAreaImage(areaImageDto);
            areaImage.UserId = UserId;
            areaImage.AreaId = areaId;
            unitOfWork.AreaImages.Add(areaImage);

            return new ResourceWrapper<AreaImageDto>( () => CreateAreaImageDto(areaImage) );
        }

        public async Task UpdateImage(Guid areaImageId, Stream imageStream, string fileExtension)
        {
            var areaImage = await GetResource(areaImageId, PermissionType.Update);
            // delete old wall image file if needed
            fsService.DeleteImage(areaImage.Url);
            // save new image to file
            var relImagePath = fsService.SaveImageStreamToFile(imageStream, fileExtension);
            areaImage.Url = relImagePath;
        }

        public async Task Delete(Guid id)
        {
            AreaImage areaImage = await GetResource(id, Common.PermissionType.Delete);
            unitOfWork.AreaImages.Remove(areaImage);
        }

        private AreaImage CreateAreaImage(AreaImageDto areaImageDto)
        {
            return new AreaImage()
            {
                Name = areaImageDto.Name,
                Url = areaImageDto.Href,
                UserId = UserId
            };
        }

        private async Task<AreaImageDto> CreateAreaImageDto(AreaImage areaImage)
        {
            var ai = new AreaImageDto()
            {
                Name = areaImage.Name,
                Href = areaImage.Url,
                Id = areaImage.Id
            };

            await SetPermissions(ai, areaImage);

            return ai;
        }
    }
}
