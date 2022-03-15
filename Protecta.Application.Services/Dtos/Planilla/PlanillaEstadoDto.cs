using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Planilla
{
    public class PlanillaEstadoDto
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
