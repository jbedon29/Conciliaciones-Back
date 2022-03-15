﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg
{
    public class DocumentoAbono
    {
        public string NumeroFactura { get; set; }
        public string FechaFactura { get; set; }
        public decimal MontoFactura { get; set; }
        public string IdNotaCredito { get; set; }
        public string FechaNotaCredito { get; set; }
        public string Estado { get; set; }
    }
}