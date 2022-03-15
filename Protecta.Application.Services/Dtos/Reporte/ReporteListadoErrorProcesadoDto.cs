using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReporteListadoErrorProcesadoDto
    {
        public string planilla { get; set; }
        public string numero_operacion { get; set; }
        public decimal monto { get; set; }
        public string fecha_deposito { get; set; }

        public string banco { get; set; }
        public string numero_cuenta { get; set; }
        public string valor { get; set; }
        public decimal saldo { get; set; }

        public Boolean isValid { get; set; }
    }
}
