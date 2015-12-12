using OutdoorSolution.Dto;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IUserInfoService : IUserResourceService, IService
    {
        Task<UserInfoDto> GetById(Guid id);

        Task Update(Guid id, UserInfoDto userInfoDto);

        Task UpdateAvatarImage(Guid wallId, Stream imageStream, string fileExtension);

        Task UpdateCoverImage(Guid wallId, Stream imageStream, string fileExtension);
    }
}
