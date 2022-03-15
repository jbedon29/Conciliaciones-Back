using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
    public class DetalleRecibo
    {
        public string Movimiento { get; set; }
        public string NroCupon { get; set; }
        public string NroRecibo { get; set; }
        public string Fecha { get; set; }
        public string IdTransacion { get; set; }
        public string DescTransacion { get; set; }
        public string FechaPago { get; set; }
        public string MontoCupon { get; set; }
        public string IdUsuario { get; set; }
        public string DescUsuario { get; set; }

    }
}
