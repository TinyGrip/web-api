﻿using Microsoft.AspNet.Identity;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dto;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Helpers;
using OutdoorSolution.Models;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace OutdoorSolution.Controllers
{
    public class AreasController : UserResourceController<Area, AreaDto>
    {
        private readonly IAreaService areaService;

        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
            this.areaService.UserId = User.Identity.GetUserId();
        }

        public async override Task<IHttpActionResult> GetById(Guid id)
        {
            var route = await areaService.GetById(id);
            return Ok(route);
        }

        public async Task<IHttpActionResult> Get([FromUri]PagingParams param)
        {
            var areas = await areaService.Get(param);
            var responsePage = CreatePage<AreaDto>(areas, param);
            return Ok(responsePage);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostArea(AreaDto areaDto)
        {
            var areaWrapper = areaService.Create(areaDto);
            await UnitOfWork.SaveChangesAsync();

            var area = areaWrapper.GetValue();

            return Created(String.Empty, areaDto);
        }

        [Authorize]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]AreaDto areaDto)
        {
            await areaService.Update(id, areaDto);
            await UnitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await areaService.Delete(id);
            await UnitOfWork.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Link GetPagingLink(PagingParams pagingParams)
        {
            return Url.Link<AreasController>(c => c.Get(pagingParams));
        }
    }
}