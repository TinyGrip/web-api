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
using OutdoorSolution.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Helpers;

namespace OutdoorSolution.Controllers
{
    public class PreviewAreasController : PagingController
    {
        private readonly ApplicationDbContext db;
        private readonly AreaMapper areaMapper;

        public PreviewAreasController(ApplicationDbContext dbContext, AreaMapper areaMapper)
        {
            db = dbContext;
            this.areaMapper = areaMapper;
        }

        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            // TODO: check if to use eager loading!
            param.TotalAmount = db.Areas.Count();
            if (param.TotalAmount == 0)
            {
                return NotFound();
            }

            // create lighter request, by using preview model
            // TODO: check if this has actually better performance
            var areas = await db.Areas.OrderByDescending(a => a.Name)
                                      .Skip(param.Skip)
                                      .Take(param.Take)
                                      .Select(a => new PreviewArea()
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Rating = a.Rating,
                                          Location = a.Location
                                      })
                                      .ToListAsync();

            var areaDtos = areas.Select(a => areaMapper.CreatePreviewAreaDto(a, Url)).ToList();
            areas = null;

            var responsePage = CreatePage<AreaDto>(areaDtos, param);
            return Ok(responsePage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // no need to implement. should never be executed
        public override Task<IHttpActionResult> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override Dto.Infrastructure.Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<PreviewAreasController>(c => c.Get(pagingParams));
        }
    }
}