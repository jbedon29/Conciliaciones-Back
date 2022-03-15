using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosConsultaReporteListado
    {
        public long idProducto
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
