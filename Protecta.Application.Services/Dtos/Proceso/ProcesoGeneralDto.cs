using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Proceso
{
    public class ProcesoGeneralDto
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
