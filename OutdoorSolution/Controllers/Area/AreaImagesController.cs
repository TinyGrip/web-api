using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;    
using OutdoorSolution.Mapping;
using OutdoorSolution.Dto;
using OutdoorSolution.Helpers;

namespace OutdoorSolution.Controllers
{
    [Route("api/Areas/{areaId}/Images")]
    public class AreaImagesController : ApiController
    {
        private const string AREA_IMAGE_ROUTE = "api/Areas/Images/{id}";

        private readonly ApplicationDbContext db;
        private readonly AreaMapper areaMapper;

        public AreaImagesController(ApplicationDbContext dbContenxt, AreaMapper areaMapper)
        {
            db = dbContenxt;
            this.areaMapper = areaMapper;
        }

        [Route(AREA_IMAGE_ROUTE)]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            AreaImage areaImage = await db.AreaImages.FindAsync(id);
            if (areaImage == null)
            {
                return NotFound();
            }

            var areaImageDto = areaMapper.CreateAreaImageDto(areaImage);
            return Ok(areaImageDto);
        }

        public async Task<IHttpActionResult> GetByAreaId(Guid areaId)
        {
            var areaImages = await db.AreaImages.Where(ai => ai.AreaId == areaId).ToListAsync();
            if (areaImages.Count == 0)
            {
                return NotFound();
            }

            var areaImagesDtos = areaImages.Select(ai => areaMapper.CreateAreaImageDto(ai));
            areaImages = null; // ??

            return Ok(areaImagesDtos);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostAreaImage(Guid areaId, [FromBody]AreaImageDto areaImageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var area = await db.Areas.FindAsync(areaId);
            if (area == null)
            {
                return NotFound(); // TODO: think if this response is proper
            }

            var areaImage = areaMapper.CreateAreaImage(areaImageDto);
            areaImage.AreaId = area.Id;
            db.AreaImages.Add(areaImage);

            await db.SaveChangesAsync();

            areaImageDto = areaMapper.CreateAreaImageDto(areaImage);
            // TODO: check the correctness of forming response in such way
            return Created(Url.Link<AreaImagesController>(c => c.GetById(areaImage.Id)).Href, areaImageDto);
        }

        [Route(AREA_IMAGE_ROUTE)]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAreaImage(Guid id)
        {
            AreaImage areaImage = await db.AreaImages.FindAsync(id);
            if (areaImage == null)
            {
                return NotFound();
            }

            db.AreaImages.Remove(areaImage);
            await db.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}