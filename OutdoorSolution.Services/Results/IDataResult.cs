using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Results
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
