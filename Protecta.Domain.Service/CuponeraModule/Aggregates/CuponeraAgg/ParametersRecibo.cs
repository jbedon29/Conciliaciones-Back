using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
    public class ParametersRecibo
    {
        public int idTransacion { get; set; }
        public string idRamo { get; set; }
        public string idProducto { get; set; }
        public string idPoliza { get; set; }
        public string idCertificado { get; set; }
        public string NroRecibo { get; set; }
        public string NroCuponera { get; set; }
        public string NroMovimiento { get; set; }
        public string Monto { get; set; } 
        public string MontoInicial { get; set; }
        public string FechaPago { get; set; }
        public string NroCupones { get; set; }
        public string NroCupon { get; set; }
        public string UserCode { get; set; }
        public string Key { get; set; }
    }
}
