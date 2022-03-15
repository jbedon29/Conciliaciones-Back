using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReporteDepositoPendienteDto
    {
        //public long IdDepositArchivo { get; set; }

        public long IdDeposito { get; set; }

        public string FechaDeposito { get; set; }       

        public decimal Monto { get; set; }

        public decimal Saldo { get; set; }

        public string NumeroOperacion { get; set; }

        public string NombreArchivo { get; set; }

        public string Cuenta { get; set; }

        public string Banco { get; set; }
    }
}
