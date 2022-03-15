using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Planilla
{
    public class DetallePlanillaPagoDto
    {
        public int IdDetallePlanillaPago
        {
            get; set;
        }

        public int IdPlanilla
        {
            get;
            set;
        }

        public int IdMoneda
        {
            get;
            set;
        }

        public string DcMonto
        {
            get;
            set;
        }

        public int IdTipomediopago
        {
            get;
            set;
        }

        public int IdBanco
        {
            get;
            set;
        }

        public int IdCuentaBanco
        {
            get;
            set;
        }

        public string VcNumerooperacion
        {
            get;
            set;
        }

        public string IddgEstado
        {
            get;
            set;
        }

        public string IddgEstadoDetPlanilla
        {
            get;
            set;
        }

        public string IddgEstadoDeposito
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
        //Agregado 17/07/2018
        public DateTime DtFechaoperacion
        {
            get;
            set;
        }
    }
}
