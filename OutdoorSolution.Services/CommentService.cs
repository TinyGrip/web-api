using OutdoorSolution.Dal;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Interfaces;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Services.Exceptions;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto.Infrastructure;

namespace OutdoorSolution.Services
{
    public class CommentService : ICommentService
    {
        readonly IUnitOfWork unitOfWork;
        readonly TGUserManager userManager;

        public CommentService(IUnitOfWork unitOfWork, TGUserManager userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<List<CommentDto>> GetByArea(Guid areaId, IPagingData pagingData)
        {   
            return await GetBySubjectId<AreaComment>(areaId, pagingData);
        }

        public async Task<List<CommentDto>> GetByRoute(Guid routeId, IPagingData pagingData)
        {
            return await GetBySubjectId<RouteComment>(routeId, pagingData);
        }

        public ResourceWrapper<CommentDto> CreateForArea(Guid areaId, CommentDto commentDto)
        {
            var comment = CreateComment<AreaComment>(areaId, commentDto);
            unitOfWork.AreaComments.Add(comment);
            return new ResourceWrapper<CommentDto>(() => CreateCommentDto(comment));
        }
        
        public ResourceWrapper<CommentDto> CreateForRoute(Guid routeId, CommentDto commentDto)
        {
            var comment = CreateComment<RouteComment>(routeId, commentDto);
            unitOfWork.RouteComments.Add(comment);
            return new ResourceWrapper<CommentDto>(() => CreateCommentDto(comment));
        }

        public async Task DeleteAreaComment(Guid id)
        {
            await DeleteComment<AreaComment>(id);
        }

        public async Task DeleteRouteComment(Guid id)
        {
            await DeleteComment<RouteComment>(id);
        }

        private async Task DeleteComment<T>(Guid commentId) where T : Comment
        {
            if (!await userManager.IsInRoleAsync(UserId, RoleNames.Admin))
            {
                throw new AccessDeniedException("Only administrator can delete comments");
            }

            var comment = await unitOfWork.Set<T>().FindAsync(commentId);
            unitOfWork.Set<T>().Remove(comment);
        }

        private async Task<List<CommentDto>> GetBySubjectId<T>(Guid subjectId, IPagingData pagingData) where T : Comment
        {
            var q = unitOfWork.Set<T>().Where(x => x.SubjectId == subjectId);
            pagingData.TotalAmount = q.Count();
            if (pagingData.TotalAmount == 0)
            {
                return new List<CommentDto>();
            }

            var comments = await q.OrderByDescending(x => x.Created)
                                      .Skip(pagingData.Skip)
                                      .Take(pagingData.Take)
                                      .ToListAsync();

            var commentDtos = await Utils.WhenAllSeq(comments.Select(x => CreateCommentDto(x)));
            return commentDtos;
        }

        private async Task<CommentDto> CreateCommentDto(Comment comment)
        {
            // allow to delete comments only to admins
            bool isAdmin = false;
            if (!String.IsNullOrEmpty(UserId))
                isAdmin = await userManager.IsInRoleAsync(UserId, RoleNames.Admin);
            
            var commentDto = new CommentDto()
            {
                Id = comment.Id,
                Text = comment.Text,
                Created = comment.Created,
                Permissions = new PermissionsData()
                {
                    CanDelete = isAdmin
                }
            };

            return commentDto;
        }

        private T CreateComment<T>(Guid subjectId, CommentDto commentDto) where T : Comment, new()
        {
            if (String.IsNullOrWhiteSpace(commentDto.Text))
            {
                throw new BadDataException();
            }

            var comment = new T()
            {
                Text = commentDto.Text,
                Created = DateTime.UtcNow,
                UserId = UserId,
                SubjectId = subjectId
            };

            return comment;
        }

        public string UserId
        {
            get;
            set;
        }
    }
}
