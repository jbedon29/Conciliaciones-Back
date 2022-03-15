using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteReciboPayDto
    {
        public string sContratante { get; set; }
        public string sProducto { get; set; }
        public int nPoliza { get; set; }
        public double nRecibo { get; set; }
        public string ini_vigencia { get; set; }
        public string fin_vigencia { get; set; }
        public string sMoneda { get; set; }
        public decimal nImporte { get; set; }
        public long nroEnvio { get; set; }

        public string sComprobante { get; set; }
        public string sEstado { get; set; }
        public string dlimite_pago { get; set; }
        public int NEstadoDetLot { get; set; }
        public int NStatusPre { get; set; }
        public bool isCheck { get; set; }
        public double nPrimerRecibo { get; set; }
        public bool checkDes { get; set; }
        public double nIdStatus { get; set; }

        


    }
}
