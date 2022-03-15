using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosPlanillaDto
    {
        public long Planilla { get; set; }
        public string NumeroOperacion { get; set; }

        public string NumeroOperacionDetalle { get; set; }
        public string Usuario { get; set; }

        public string FechaDeposito { get; set; }
    }
}
