using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg
{
    public class ReporteListadoError
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
