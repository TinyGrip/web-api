using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Helpers;
using OutdoorSolution.Dto.Infrastructure;
using OutdoorSolution.Models;
using OutdoorSolution.Dal;
using OutdoorSolution.Services;

namespace OutdoorSolution.Controllers
{
    public abstract class PagingController : ApiController
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public abstract Task<IHttpActionResult> GetById(Guid id);

        // TODO: exclude TOTAL AMOUNT
        protected abstract Link GetPagingLink(PagingParams pagingParams);

        protected Page<T> CreatePage<T>(IEnumerable<T> items, PagingParams param)
            where T : PageItem
        {
            var page = new Page<T>();
            page.Items = items;
            page.TotalAmount = param.TotalAmount;

            if (param.Skip + param.Take < param.TotalAmount)
            {
                var nextParam = new PagingParams()
                {
                    Skip = Math.Min(param.Skip + param.Take, param.TotalAmount),
                    Take = param.Take
                };
                page.Next = GetPagingLink(nextParam);
            }

            if (param.Skip != 0)
            {
                var prevParam = new PagingParams()
                {
                    Skip = Math.Max(param.Skip - param.Take, 0),
                    Take = param.Take
                };
                page.Prev = GetPagingLink(prevParam);
            }

            return page;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
