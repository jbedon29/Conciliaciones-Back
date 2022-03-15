using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg
{
    public class Deposito
    {
        public long IdDepositArchivo { get; set; }

        public long IdDeposito { get; set; }

        public DateTime FechaDeposito { get; set; }

        public long IdEstado { get; set; }

        public decimal Monto { get; set; }

        public decimal Saldo { get; set; }

        public string NumeroOperacion { get; set; }

        public long IdMoneda { get; set; }

        //Agregado 07-08-2018
        public int IdTipoMedioPago { get; set; }

        public long IdDepositoAsociado { get; set; }

        public string Extorno { get; set; }
    }
}
