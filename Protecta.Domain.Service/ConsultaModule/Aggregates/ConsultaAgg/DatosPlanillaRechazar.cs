using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosPlanillaRechazar
    {
        public long Planilla { get; set; }
        public string Usuario { get; set; }
        public string observacion { get; set; }
    }
}
