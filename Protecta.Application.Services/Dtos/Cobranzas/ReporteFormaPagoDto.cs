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
        public string recibo { get; set; }
        public string Fecha_ope { get; set; }
        public string sel_auto { get; set; }
        public string forma_pago { get; set; }
        public int tipo { get; set; }

    }
}
