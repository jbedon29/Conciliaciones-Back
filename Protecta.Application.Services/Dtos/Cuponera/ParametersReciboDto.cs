using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cuponera
{
    public class ParametersReciboDto
    {
        public int idTransacion { get; set; }
        public string NroRecibo { get; set; }
        public string NroCuponera { get; set; }
        public string NroMovimiento { get; set; }
        public string Monto { get; set; }
        public string MontoInicial { get; set; }
        public string NroCupones { get; set; }
        public string UserCode { get; set; }
        public string Key { get; set; }
    }
}
