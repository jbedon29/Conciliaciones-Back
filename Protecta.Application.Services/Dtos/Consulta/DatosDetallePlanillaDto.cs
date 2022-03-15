using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatoDetallesPlanillaDto
    {

        public string numeroOperacion { get; set; }
        public string fechaDeposito{ get; set; }
        public string numeroCuenta { get; set; }
        public string monto { get; set; }
        public string valor { get; set; }

        // agregado  agregado 09/08/2021  en el pop Up
        public string saldo { get; set; }


        public string fecha_ini { get; set; }
        public string fecha_fin { get; set; }

    }
}
