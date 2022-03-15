using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteReciboDetallePayDto
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
