using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Deposito
{
    public class DepositoDto
    {
        public long IdDepositArchivo { get; set; }

        public long IdDeposito { get; set; }

        public string FechaDeposito { get; set; }

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
