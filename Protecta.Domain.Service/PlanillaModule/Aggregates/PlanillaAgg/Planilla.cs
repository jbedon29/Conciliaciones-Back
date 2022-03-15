using System;
using System.Collections.Generic;

namespace Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg
{
    public class Planilla
    {
        public int IdPlanilla
        {
            get; set;
        }

        public string VcDescripcion
        {
            get;
            set;
        }

        public string DcTotal
        {
            get;
            set;
        }

        public DateTime DtFechaPlanilla
        {
            get;
            set;
        }

        public int IdCanal
        {
            get;
            set;
        }

        public string IdPuntoventa
        {
            get;
            set;
        }

        public string IddgEstadoPlanilla
        {
            get;
            set;
        }

        public string IddgEstadoProenv
        {
            get;
            set;
        }

        public string IddgEstado
        {
            get;
            set;
        }

        public int IdProducto
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

        public IEnumerable<DetallePlanillaCobro> DetallePlanillacobro
        {
            get;
            set;
        }

        public IEnumerable<DetallePlanillaPago> DetallePlanillapago
        {
            get;
            set;
        }

        //Control de cambio 1.1
        public string CodigoCanal
        {
            get;
            set;
        }

        public string DescripcionCanal
        {
            get;
            set;
        }
    }
}
