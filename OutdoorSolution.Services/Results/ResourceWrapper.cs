using OutdoorSolution.Domain.Models.Infrastructure;
using OutdoorSolution.Dto.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Results
{
    public class ResourceWrapper<T> where T: PageItem
    {
        private Func<Task<T>> dtoCreator;

        public ResourceWrapper(Func<Task<T>> dtoCreator)
        {
            this.dtoCreator = dtoCreator;
        }

        public Task<T> GetValue()
        {
            return dtoCreator();
        }
    }
}
