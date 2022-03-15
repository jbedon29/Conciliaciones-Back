using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class DatosConsultaContratantePay
    {
        public long IdRamoPay { get;  set; }
        public long IdProductoPay { get; set; }
        public long IdPoliza { get; set; }
    }
}
