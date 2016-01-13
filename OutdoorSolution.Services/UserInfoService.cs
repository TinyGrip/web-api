using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services
{
    public class UserInfoService : IUserInfoService
    {
        readonly TGUserManager userManager;
        readonly IUnitOfWork unitOfWork;
        readonly IFileSystemService fsService;
        readonly IRouteGradeService routeGradeService;

        public UserInfoService(TGUserManager userManager, IUnitOfWork unitOfWork, IFileSystemService fileSystemService, IRouteGradeService routeGradeService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.fsService = fileSystemService;
            this.routeGradeService = routeGradeService;
        }

        public async Task<UserInfoDto> GetById(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            return CreateDto(user);
        }

        public async Task Update(Guid id, UserInfoDto userInfoDto)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (userInfoDto.CoverHref != null && user.CoverImage != userInfoDto.CoverHref)
            {
                fsService.DeleteImage(user.CoverImage);
                user.CoverImage = userInfoDto.CoverHref;
            }
            if (userInfoDto.AvatarHref != null && user.AvatarImage != userInfoDto.AvatarHref)
            {
                fsService.DeleteImage(user.AvatarImage);
                user.AvatarImage = userInfoDto.AvatarHref;
            }

            if (userInfoDto.FreeClimbingGradesSystem.HasValue)
                user.FreeClimbingGradesSystem = userInfoDto.FreeClimbingGradesSystem.Value;
            if (userInfoDto.BoulderingGradesSystem.HasValue)
                user.BoulderingGradesSystem = userInfoDto.BoulderingGradesSystem.Value;

            // TODO: think about email change
        }

        public async Task UpdateAvatarImage(Guid userId, Stream imageStream, string fileExtension)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            // delete old wall image file if needed
            fsService.DeleteImage(user.AvatarImage);

            // save new image to file
            var relImagePath = fsService.SaveImageStreamToFile(imageStream, fileExtension);
            user.AvatarImage = relImagePath;
        }

        public async Task UpdateCoverImage(Guid userId, Stream imageStream, string fileExtension)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            // delete old wall image file if needed
            fsService.DeleteImage(user.CoverImage);

            // save new image to file
            var relImagePath = fsService.SaveImageStreamToFile(imageStream, fileExtension);
            user.CoverImage = relImagePath;
        }

        public string UserId { get; set; }

        private UserInfoDto CreateDto(ApplicationUser appUser)
        {
            var userDto = new UserInfoDto()
            {
                Id = new Guid(appUser.Id),
                Email = appUser.Email,
                AvatarHref = appUser.AvatarImage,
                CoverHref = appUser.CoverImage,
                BoulderingGradesSystem = appUser.BoulderingGradesSystem,
                FreeClimbingGradesSystem = appUser.FreeClimbingGradesSystem,
                BoulderingGrades = routeGradeService.GetBoulderingGrades(appUser.BoulderingGradesSystem),
                FreeClimbingGrades = routeGradeService.GetFreeClimbingGrades(appUser.FreeClimbingGradesSystem)
            };

            return userDto;
        }
    }
}
