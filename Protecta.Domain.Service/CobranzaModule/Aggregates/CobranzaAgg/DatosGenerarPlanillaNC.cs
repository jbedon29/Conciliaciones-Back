using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class DatosGenerarPlanillaNC
    {
        public int recibo { get; set; }
        public int monto { get; set; }
        public int idTipoFP { get; set; }
        public int nUserCode { get; set; }
        public int numOperacion { get; set; }
        public int idBanco { get; set; }
        public string cuenta { get; set; }
        public string fecha_ope { get; set; }

    }
}
