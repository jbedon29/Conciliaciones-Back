using System;
using System.Collections.Generic;

namespace Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg
{
    public class Conciliacion
    {
        public int IdConciliacionplanilla
        {
            get;
            set;
        }

        public int IdPlanilla
        {
            get;
            set;
        }

        public int IdDeposito
        {
            get;
            set;
        }

        public string DtFechaconciliacion
        {
            get;
            set;
        }

        public string DCtotal
        {
            get;
            set;
        }

        public string DCcomision
        {
            get;
            set;
        }

        public string DCmontoneto
        {
            get;
            set;
        }

        public int IddgEstado
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

        public List<ConciliacionDetalle> Conciliaciondetalle
        {
            get;
            set;
        }
    }
}
