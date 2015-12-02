using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Results
{
    public class DataResult<T> : IDataResult<T>
    {
        public DataResult() { }

        public DataResult(T data)
        {
            Data = data;
            Code = ResultCodes.OK;
        }

        public DataResult(ResultCodes code)
        {
            Code = code;
        }

        public ResultCodes Code { get; set; }

        public T Data { get; set; }
    }
}
