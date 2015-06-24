using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OutdoorSolution.Helpers;

namespace OutdoorSolution.Controllers
{
    public abstract class PagingController<C> : ApiController where C: PagingController<C>
    {
        public PagingController()
        {
            DefaultPagingParams = new PagingParams()
            {
                Take = 10
            };
        }

        public abstract Task<IHttpActionResult> GetById(Guid id);

        // TODO: add "TotalAmount" excluding
        //public abstract Task<IHttpActionResult> Get([FromUri]PagingParams param);

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
                //page.Next = Url.Link<C>(c => c.Get(nextParam));
            }

            if (param.Skip != 0)
            {
                var prevParam = new PagingParams()
                {
                    Skip = Math.Max(param.Skip + param.Take, 0),
                    Take = param.Take
                };
                //page.Prev = Url.Link<C>(c => c.Get(prevParam));
            }

            return page;
        }

        protected PagingParams DefaultPagingParams { get; private set; }
    }
}
