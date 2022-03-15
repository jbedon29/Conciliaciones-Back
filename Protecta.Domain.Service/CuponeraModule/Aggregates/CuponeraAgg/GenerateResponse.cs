using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
    public class GenerateResponse
    {
        public int P_NCODE { get; set; }
        public string P_SMESSAGE { get; set; }
        public Object data { get; set; }
    }
}
