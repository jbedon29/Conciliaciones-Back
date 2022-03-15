using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ResponseControl
    {
        public object Data { get; set; }
        public string Code { get; set; }
        public string message { get; set; }

        public ResponseControl(Status _Status)
        {
            Code = (_Status == Status.Ok) ? "0" : "1"; 
        }
        public enum Status
        {
            Ok = 0 ,
            Fail = 1 
        }
    }
    
}
