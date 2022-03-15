using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReporteMacroConsultaDto
    {

        public string numero_operacion { get; set; }
        public string fecha_conciliacion { get; set; }
        public string monto_bruto_dep { get; set; }
        public string monto_neto_dep { get; set; }
        public string monto_comis_dep { get; set; }
        public string monto_ocargo_dep { get; set; }
        public string deposito { get; set; }
        public string deposito_archivo { get; set; }
        public string monto { get; set; }
        public string saldo { get; set; }
        public string nombre_archivo { get; set; }
        public string fecha_deposito { get; set; }
        public string diferencia { get; set; }
    }
}
