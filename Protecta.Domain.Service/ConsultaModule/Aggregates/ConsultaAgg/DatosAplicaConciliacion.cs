using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosAplicaConciliacion
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

        public string Usuario { get; set; }
        public long IdTipoCobro
        {
            get;
            set;
        }
    }
}
