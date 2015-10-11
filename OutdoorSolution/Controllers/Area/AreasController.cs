﻿using System;
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
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dal;
using OutdoorSolution.Mapping;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Filters;

namespace OutdoorSolution.Controllers
{
    public class AreasController : PagingController
    {
        private readonly ApplicationDbContext db;
        private readonly AreaMapper areaMapper;

        public AreasController(ApplicationDbContext dbContenxt, AreaMapper areaMapper)
        {
            this.db = dbContenxt;
            this.areaMapper = areaMapper;
        }

        [ResponseType(typeof(AreaDto))]
        public override async Task<IHttpActionResult> GetById(Guid id)
        {
            Area area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            var areaDto = areaMapper.CreateAreaDto(area, Url);

            return Ok(areaDto);
        }

        [ResponseType(typeof(Page<AreaDto>))]
        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            // TODO: check if to use eager loading!
            param.TotalAmount = db.Areas.Count();
            if (param.TotalAmount == 0)
            {
                return NotFound();
            }

            var areas = await db.Areas.OrderByDescending(a => a.Name)
                                      .Skip(param.Skip)
                                      .Take(param.Take)
                                      .ToListAsync();
            
            // TODO: think about memory
            var areaDtos = areas.Select( a => areaMapper.CreateAreaDto(a, Url) ).ToList();
            areas = null;

            var responsePage = CreatePage<AreaDto>(areaDtos, param);
            return Ok(responsePage);
        }

        [ResponseType(typeof(void))]
        [Authorize]
        public async Task<IHttpActionResult> PutArea(Guid id, AreaDto areaDto)
        {
            Area area = await db.Areas.FindAsync(id);

            if (area == null)
                return BadRequest("Not existing area");

            areaMapper.UpdateArea(area, areaDto);

            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Area))]
        [Authorize]
        public async Task<IHttpActionResult> PostArea(AreaDto areaDto)
        {
            areaDto.Created = DateTime.Now;

            // create db model and save it
            var area = areaMapper.CreateArea(areaDto);
            db.Areas.Add(area);
            await db.SaveChangesAsync();

            // create dto model of saved area
            areaDto = areaMapper.CreateAreaDto(area, Url);

            return CreatedAtRoute("DefaultApi", new { id = area.Id }, areaDto);
        }

        [ResponseType(typeof(Area))]
        [Authorize]
        public async Task<IHttpActionResult> DeleteArea(Guid id)
        {
            Area area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            db.Areas.Remove(area);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<AreasController>(c => c.Get(pagingParams));
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