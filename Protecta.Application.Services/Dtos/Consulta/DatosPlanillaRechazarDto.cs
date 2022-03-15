using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosPlanillaRechazarDto
    {
        public long Planilla { get; set; }
        public string Usuario { get; set; }

        public string observacion { get; set; }
    }
}
