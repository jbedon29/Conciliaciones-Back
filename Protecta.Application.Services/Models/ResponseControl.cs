using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Models
{
    public class ResponseControl
    {
        public object Data { get; set; }
        public string Code { get; set; }
        //public string rValid { get; set; }

        public string message { get; set; }

        //public ResponseControl(Status _Status)
        //{
        //    Code = (_Status == Status.Ok) ? "0" : "1";
        //}
        public enum Status
        {
            Ok = 0,
            Fail = 1
        }
    }
}
