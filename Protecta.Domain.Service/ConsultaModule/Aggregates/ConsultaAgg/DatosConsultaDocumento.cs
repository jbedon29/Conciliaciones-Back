using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosConsultaDocumento
    {
        public string NumeroFactura { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
    }
}
