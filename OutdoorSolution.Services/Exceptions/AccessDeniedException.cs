﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() { }

        public AccessDeniedException(String message) : base(message) { }
    }
}
