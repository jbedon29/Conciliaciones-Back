using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ReporteReciboDetallePay
    {
        public long nroPoliza { get; set; }
        public long nroRecibo { get; set; }
        public string sFechaIniVigencia { get; set; }
        public string sFechaFinVigencia { get; set; }
        public string sEstado { get; set; }
        public string sMotivo { get; set; }
        public string sComprobante { get; set; }


    }
}
