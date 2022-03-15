using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg
{
    public class ReporteDepositoPendiente
    {
        public long IdDepositArchivo { get; set; }

        public long IdDeposito { get; set; }

        public DateTime FechaDeposito { get; set; }

        public decimal Monto { get; set; }

        public decimal Saldo { get; set; }

        public string NumeroOperacion { get; set; }

        public string  NombreArchivo { get; set; }

        public string Cuenta { get; set; }

        public string Banco { get; set; }
    }
}
