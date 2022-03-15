using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
   public class DatosConsultaExtracto
    {

        public long idProducto { get; set; }
        public long IdBanco
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
    }
}
