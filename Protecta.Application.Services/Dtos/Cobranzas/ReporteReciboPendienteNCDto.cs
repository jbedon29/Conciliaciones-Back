using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteReciboPendienteNCDto
    {
        public string ramo { get; set; }
        public string producto { get; set; }
        public string recibo { get; set; }
        public string estado { get; set; }
        public string moneda { get; set; }
        public string monto { get; set; }
        public string ini_vigen { get; set; }
        public string fin_vigen { get; set; }



    }
}
