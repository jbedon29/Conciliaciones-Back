using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReporteMacroProcesadoDto
    {       
        public string numero_operacion { get; set; }
        public string fecha_conciliacion { get; set; }
        public decimal monto_bruto_dep { get; set; }
        public decimal monto_neto_dep { get; set; }
        public decimal monto_comis_dep { get; set; }
        public decimal monto_ocargo_dep { get; set; }
        public string deposito { get; set; }
        public string deposito_archivo { get; set; }
        public decimal monto { get; set; }
        public decimal saldo { get; set; }
        public string nombre_archivo { get; set; }
        public string fecha_deposito { get; set; }
        public string diferencia { get; set; }
        public string moneda { get; set; }

    }
}
