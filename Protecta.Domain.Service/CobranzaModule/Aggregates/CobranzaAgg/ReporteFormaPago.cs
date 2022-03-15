using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ReporteFormaPago
    {
        public string documento { get; set; }
        public string contratante { get; set; }
        public string ini_vigencia { get; set; }
        public string fin_vigencia { get; set; }
        public string moneda { get; set; }
        public string monto { get; set; }
    }
}
