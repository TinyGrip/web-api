using OutdoorSolution.Dto;
using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface ICommentService: IUserResourceService, IService
    {
        Task<List<CommentDto>> GetByArea(Guid areaId, IPagingData pagingData);

        Task<List<CommentDto>> GetByRoute(Guid routeId, IPagingData pagingData);

        ResourceWrapper<CommentDto> CreateForArea(Guid areaId, CommentDto comment);

        ResourceWrapper<CommentDto> CreateForRoute(Guid routeId, CommentDto comment);

        Task DeleteAreaComment(Guid id);

        Task DeleteRouteComment(Guid id);

    }
}
