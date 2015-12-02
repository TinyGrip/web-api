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
        private Func<T> dtoCreator;

        public ResourceWrapper(Func<T> dtoCreator)
        {
            this.dtoCreator = dtoCreator;
        }

        public T GetValue()
        {
            return dtoCreator();
        }
    }
}
