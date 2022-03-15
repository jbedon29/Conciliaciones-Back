using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteFormaPagoDto
    {
        public string documento { get; set; }
        public string contratante { get; set; }
        public string ini_vigencia { get; set; }
        public string fin_vigencia { get; set; }
        public string moneda { get; set; }
        public string monto { get; set; }

    }
}
