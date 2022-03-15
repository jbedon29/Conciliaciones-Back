using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class DatosGenerarPlanillaNCDto
    {
        public  int recibo{ get; set; }
        public int monto { get; set; }
        public int idTipoFP { get; set; }
        public int nUserCode { get; set; }
        public int numOperacion { get; set; }
        public int idBanco { get; set; }
        public string cuenta { get; set; }
        public string fecha_ope { get; set; }


    }
}


