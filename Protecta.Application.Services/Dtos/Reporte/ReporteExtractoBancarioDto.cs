using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReporteExtractoBancarioDto
    {
        //public string canal { get; set; }
        public string fecha_abon { get; set; }
        public string concepto { get; set; }
        public string banco { get; set; }
        public string numero_cuenta { get; set; }
        public string operacion { get; set; }
        public decimal  monto_xaplicar { get; set; }
        public decimal monto_aplicado { get; set; }
        public decimal saldo { get; set; }
        public string moneda { get; set; }




    }
}
