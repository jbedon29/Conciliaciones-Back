using System;

namespace Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg
{
    public class ProcesoGeneral
    {
        public int IdProcesoGeneral
        {
            get;
            set;
        }

        public int IdProceso
        {
            get;
            set;
        }

        public string VcDescripcion
        {
            get;
            set;
        }

        public string IddgEstadoProcesoGeneral
        {
            get;
            set;
        }

        public DateTime DtFechacreacion
        {
            get;
            set;
        }

        public string VcUsuariocreacion
        {
            get;
            set;
        }

        public DateTime DtFechamodificacion
        {
            get;
            set;
        }

        public string VcUsuariomodificacion
        {
            get;
            set;
        }

        public string VcMensaje
        {
            get;
            set;
        }

        public string VcAmbito
        {
            get;
            set;
        }
    }
}
