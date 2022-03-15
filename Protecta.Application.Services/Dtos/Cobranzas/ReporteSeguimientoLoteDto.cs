using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteSeguimientoLoteDto
    {
        public string doc_contratante { get; set; }
        public string contratante { get; set; }
        public string poliza { get; set; }
        public string per_poliza { get; set; }
        public string comprobante { get; set; }
        public string fecha_emision { get; set; }
        public string ini_vigen_comp { get; set; }
        public string fin_vigen_comp { get; set; }
        public string ini_vigen_poli { get; set; }
        public string fin_vigen_poli { get; set; }
        public string moneda { get; set; }
        public string importe { get; set; }
        public string estado_cobro { get; set; }
        public string fecha_estado { get; set; }
        public string num_envio { get; set; }
        public string fecha_envio { get; set; }
        public string agen_vendedor { get; set; }
        public string agen_seguimiento { get; set; }
        public string recibo { get; set; }
        public string detalle_estado { get; set; }
        public string tipo_comision { get; set; }
        public string estado_envio { get; set; }
        public string nrocel_contratante { get; set; }
        public string correo_contratante { get; set; }
        public string estado_comprobante { get; set; }



    }
}













