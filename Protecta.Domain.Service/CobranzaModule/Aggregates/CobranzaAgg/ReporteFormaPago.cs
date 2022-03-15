using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ReporteFormaPago
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
