using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg
{
    public class DatosAplicacionManual
    {

        public long IdProducto
        {
            get;
            set;
        }

        public string FechaDesde
        {
            get;
            set;
        }

        public string FechaHasta
        {
            get;
            set;
        }

        public string IdPlanillas { get; set; }

        public string IdDepositos { get; set; }

        public string Usuario { get; set; }
    }
}
