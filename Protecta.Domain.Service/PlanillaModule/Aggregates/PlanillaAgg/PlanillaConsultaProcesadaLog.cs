using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg
{
    public class PlanillaConsultaProcesadaLog
    {
       
        public long planilla
        {
            get;
            set;
        }
        public long procesoGeneral
        {
            get;
            set;
        }
        public string fechaProceso
        {
            get;
            set;
        }
        public string logError
        {
            get;
            set;
        }
    }
}
