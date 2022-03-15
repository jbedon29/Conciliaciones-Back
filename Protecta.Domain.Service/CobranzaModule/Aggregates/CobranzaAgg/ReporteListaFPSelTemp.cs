using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ReporteListaFPSelTemp
    {
        public string forma_pago { get; set; }
        public string operacion { get; set; }
        public string monto { get; set; }
        public string fecha_ope { get; set; }
        public string recibo { get; set; }
        public string banco { get; set; }
        public string cuenta { get; set; }
        public string moneda { get; set; }
        public int tipo { get; set; }
    }
}
