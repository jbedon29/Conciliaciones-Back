using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class State_voucher : CobranzaVoucher
    {
        public string SCAMPO { get; set; }
        public string SVALOR { get; set; }
        public string SMENSAJE { get; set; }
        public string SGRUPO { get; set; } 
        public string Resultado { get; set; }
        public string status { get; set; }
        public string Application { get; set; }
    }
}
