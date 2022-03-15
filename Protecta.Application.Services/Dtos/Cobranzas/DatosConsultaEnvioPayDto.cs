using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class DatosConsultaReciboPayDto
    {

        public string contratante { get; set; }
        public long IdPoliza { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public long IdProductoPay { get; set; }
        public long idRamo { get; set; }
        public int Estado { get; set; }
        public int moneda { get; set; }
        public long recibo { get; set; }
        public int nroEnvio { get; set; }
        public int idComprobante { get; set; }
        public string sComprobante { get; set; }




    }
}
