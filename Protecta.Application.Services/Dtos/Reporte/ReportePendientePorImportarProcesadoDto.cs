using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReportePendientePorImportarProcesadoDto
    {
        public int planilla { get; set; }
        public string fecha_registro { get; set; }
        public int cantidad { get; set; }
        public decimal monto_total { get; set; }
        public string error_planilla { get; set; }
        public string error_plataforma_digital { get; set; }
        public string fecha_error_plataforma { get; set; }
        public int cod_producto { get; set; }
        public string des_producto { get; set; }
        public string medio_pago { get; set; }
        public string moneda { get; set; }

    }
}
