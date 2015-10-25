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
    /// Base class for core climbing resources
    /// </summary>
    public abstract class UserResourceController<T, TDto> : PagingController
        where T : class, IUserResource
        where TDto : PageItem
    {
        protected readonly PermissionsService permissionsService;

        public UserResourceController(ApplicationDbContext dbContext, PermissionsService permissionsService)
            : base(dbContext)
        {
            this.permissionsService = permissionsService;
        }

        public override async Task<IHttpActionResult> GetById(Guid id)
        {
            var resource = await db.Set<T>().FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            var areaDto = CreateDto(resource);

            return Ok(areaDto);
        }

        [Authorize]
        public async Task<IHttpActionResult> Put(Guid id, TDto resourceDto)
        {
            T resource = await db.Set<T>().FindAsync(id);
            if (resource == null)
                return NotFound();

            if (!this.permissionsService.CanUserModifyResource(User, resource))
                return StatusCode(HttpStatusCode.Forbidden);

            Update(resource, resourceDto);

            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            T resource = await db.Set<T>().FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            if (!this.permissionsService.CanUserDeleteResource(User, resource))
                return StatusCode(HttpStatusCode.Forbidden);

            db.Set<T>().Remove(resource);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected abstract TDto CreateDto(T resource);

        protected abstract void Update(T resource, TDto resourceDto);
    }
}
