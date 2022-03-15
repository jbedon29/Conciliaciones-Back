using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
    public class Cupon
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
