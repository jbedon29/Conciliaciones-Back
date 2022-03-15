using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReporteComprobantesPorPlanillaProcesadoDto
    {       
        public string poliza { get; set; }
        public string nro_planilla { get; set; }
        public string estado_planilla { get; set; }
        public decimal monto_total_planilla { get; set; }
        public decimal comision { get; set; }
        public string cod_producto { get; set; }
        public string des_producto { get; set; }
        public string nro_operacion { get; set; }
        public string recibo { get; set; }
        public string tipo_comprobante { get; set; }
        public string nro_comprobante { get; set; }
        public decimal monto_bruto_comp { get; set; }
        public decimal monto_neto_comp { get; set; }
        public decimal igv_comp { get; set; }
        public decimal derecho_emision { get; set; }
        public string id_tipo_medio_pago { get; set; }
        public string des_medio_pago { get; set; }
        public string factura { get; set; }
        public string estado_comp { get; set; }
        public string fecha_liquidacion_comp { get; set; }
        public string moneda { get; set; }


    }
}
