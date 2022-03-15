using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cuponera
{
    public class CuponDto
    {
        public string skey { get; set; }
        public string nrocuponera { get; set; }
        public string nroCupon { get; set; }
        public string mroRecibo { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string fechaPago { get; set; }
        public string montoCupon { get; set; }
        public string estado { get; set; }
    }
}
