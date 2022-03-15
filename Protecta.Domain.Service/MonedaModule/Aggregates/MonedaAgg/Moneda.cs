using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.MonedaModule.Aggregates.MonedaAgg
{
    public class Moneda
    {
        public long IdMoneda { get; set; }

        public string Descripcion { get; set; }

        public string Codigo { get; set; }
    }
}
