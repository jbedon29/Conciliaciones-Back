using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Entidad
{
    public class CuentaDto
    {
        public long IdCuenta
        {
            get;
            set;
        }

        public long IdEntidad
        {
            get;
            set;
        }

        public string NumeroCuenta
        {
            get;
            set;
        }

        public long IdMoneda
        {
            get;
            set;
        }

        public string CodigoMoneda
        {
            get; set;
        }
    }
}
