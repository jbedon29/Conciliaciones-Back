using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Planilla
{
    public class PlanillaConsultaProcesadaLogDto
    {
        //Control de cambio 1.1
        public long planilla { get; set; }
        public long procesoGeneral { get; set; }
        public string fechaProceso { get; set; }
        public string logError { get; set; }
    }
}
