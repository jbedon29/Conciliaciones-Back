using System;

namespace Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg
{
    public class PlanillaEstado
    {
        public int IdPlanillaestado
        {
            get;
            set;
        }

        public int IdPlanilla
        {
            get;
            set;
        }

        public int IddgEstadoplanilla
        {
            get;
            set;
        }

        public int IddgEstado
        {
            get;
            set;
        }

        public string DtFechaproceso
        {
            get;
            set;
        }

        public string DtFechacreacion
        {
            get;
            set;
        }

        public string VcUsuariocreacion
        {
            get;
            set;
        }

        public string DtFechamodificacion
        {
            get;
            set;
        }

        public string VcUsuariomodificacion
        {
            get;
            set;
        }
    }
}
