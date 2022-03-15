using System;

namespace Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg
{
    public class LogProcesoGeneral
    {
        public int IdLogProcesoGeneral
        {
            get;
            set;
        }

        public int IdProcesoGeneral
        {
            get;
            set;
        }

        public DateTime DtFechaproceso
        {
            get;
            set;
        }

        public string VcMensaje
        {
            get;
            set;
        }

        public string IdEstadoProceso
        {
            get;
            set;
        }

        public string VcAmbito
        {
            get;
            set;
        }

        public string VcUsuario
        {
            get;
            set;
        }

        public string IddgEstado
        {
            get;
            set;
        }

        public DateTime DtFechacreacion
        {
            get;
            set;
        }

        public string VcUsuarioCreacion
        {
            get;
            set;
        }

        public DateTime DtFechamodificacion
        {
            get;
            set;
        }

        public string VcUsuarioModificacion
        {
            get;
            set;
        }
    }
}
