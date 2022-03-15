using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg
{
    public class PlanillaDetalle
    {
        public int IdPlanillaDetalle
        {
            get;
            set;
        }

        public int IdPlanilla
        {
            get;
            set;
        }

        public int IdRamo
        {
            get;
            set;
        }

        public int IdProducto
        {
            get;
            set;
        }

        public long VcNumeropoliza
        {
            get;
            set;
        }

        public long VcNumerocertificado
        {
            get;
            set;
        }

        public long IdProforma
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

        public string VcNumerooperacion
        {
            get;
            set;
        }

        public int IdBanco
        {
            get;
            set;
        }

        public string IdCuentaBanco
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
    }
}
