using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Models;
using OutdoorSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace OutdoorSolution.Controllers
{
    /// <summary>
    /// Base class for CRUD operations with core resources, created by users
    /// </summary>
    public abstract class UserResourceController<T, TDto> : PagingController
        where T : class, IUserResource
        where TDto : PageItem
    {
    }
}
