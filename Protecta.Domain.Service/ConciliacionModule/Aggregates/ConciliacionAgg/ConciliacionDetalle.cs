using System;

namespace Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg
{
    public class ConciliacionDetalle
    {
        public int IdConciliacionplanilla
        {
            get;
            set;
        }

        public int IdDetalleplanilla
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

        public int iddgEstado
        {
            get;
            set;
        }

        public string DCmontopagado
        {
            get;
            set;
        }
    }
}
