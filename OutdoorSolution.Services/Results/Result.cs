using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Results
{
    public class Result : IResult
    {
        public Result(ResultCodes result)
        {
            Code = result;
        }

        public ResultCodes Code
        {
            get;
            set;
        }
    }
}
