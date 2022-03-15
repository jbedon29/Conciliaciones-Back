using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class DatosConsultaEnvioPayDto
    {
        public long IdPoliza { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public long IdProductoPay { get; set; }
        public long idRamo { get; set; }
        public int Estado { get; set; }
        public int nroEnvio { get; set; }

    }
}
