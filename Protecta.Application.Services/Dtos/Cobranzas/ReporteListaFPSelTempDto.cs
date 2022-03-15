using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteListaFPSelTempDto
    {
        public string forma_pago { get; set; }
        public string operacion { get; set; }
        public string monto { get; set; }
        public string fecha_ope { get; set; }
        public string recibo { get; set; }
        public string banco { get; set; }
        public string cuenta { get; set; }
        public string moneda { get; set; }
        public int tipo { get; set; }

    }
}
