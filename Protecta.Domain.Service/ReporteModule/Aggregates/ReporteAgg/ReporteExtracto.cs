using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg
{
    public class ReporteExtracto
    {
        //public string canal { get; set; }
        public string fecha_abon { get; set; }
        public string concepto { get; set; }
        public string banco { get; set; }
        public string numero_cuenta { get; set; }
        public string operacion { get; set; }
        public decimal monto_xaplicar { get; set; }
        public decimal monto_aplicado { get; set; }
        public decimal saldo { get; set; }
        public string moneda { get; set; }

    }
}
