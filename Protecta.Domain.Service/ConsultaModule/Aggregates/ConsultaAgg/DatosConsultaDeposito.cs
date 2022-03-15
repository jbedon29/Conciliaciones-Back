using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosConsultaDeposito
    {
        public long IdBanco { get; set; }

        public long IdCuenta { get; set; }

        public long IdMoneda { get; set; }

        public long IdProducto { get; set; }

        public string FechaDesde { get; set; }

        public string FechaHasta { get; set; }
    }
}
