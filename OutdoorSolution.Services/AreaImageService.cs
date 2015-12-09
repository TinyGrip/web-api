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

namespace OutdoorSolution.Services
{
    public class AreaImageService : UserResourceService<AreaImage>, IAreaImageService
    {
        public AreaImageService(IUnitOfWork unitOfWork, TGUserManager userManager)
            : base(unitOfWork, userManager)
        {
        }

        public async Task<AreaImageDto> GetById(Guid id)
        {
            var areaImage = await GetResource(id, Common.PermissionType.Read);
            var areaImageDto = CreateAreaImageDto(areaImage);
            return areaImageDto;
        }

        public async Task<IEnumerable<AreaImageDto>> GetByArea(Guid areaId)
        {
            var areaImages = await unitOfWork.AreaImages.Where(ai => ai.AreaId == areaId).ToListAsync();
            var areaImagesDtos = areaImages.Select(ai => CreateAreaImageDto(ai));
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

            return new ResourceWrapper<AreaImageDto>( () => Task.FromResult(CreateAreaImageDto(areaImage)) );
        }

        public async Task Delete(Guid id)
        {
            AreaImage areaImage = await GetResource(id, Common.PermissionType.Delete);
            unitOfWork.AreaImages.Remove(areaImage);
        }

        internal static AreaImage CreateAreaImage(AreaImageDto areaImageDto)
        {
            return new AreaImage()
            {
                Name = areaImageDto.Name,
                Url = areaImageDto.Href
            };
        }

        internal static AreaImageDto CreateAreaImageDto(AreaImage areaImage)
        {
            return new AreaImageDto()
            {
                Name = areaImage.Name,
                Link = new Link(),
                Href = areaImage.Url
            };
        }
    }
}
