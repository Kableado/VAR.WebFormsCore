using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrummer.Code.Entities
{
    public class OperationStatus
    {
        public bool IsOK { get; set; }
        public string Message { get; set; }
        public string ReturnValue { get; set; }
    }
}