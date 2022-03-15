using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class Planilla
    {
        public bool Resultado { get; set; } = true;
        public string MensajeError { get; set; }
        public string RutaTrama { get; set; }
        public List<Proforma> ListaProforma { get; set; }
    }

    public class Proforma
    {
        // public string NumeroCuenta { get; set; }
        public string NombreContratante { get; set; }
        public string Documento { get; set; }
        public string DescTipoDoc { get; set; }
        public string CodigoProforma { get; set; }
        public string NumeroRecibo { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaEmision { get; set; }
        public string Importe { get; set; }

        // add for pA
        public string Ramo { get; set; }
        public string Producto { get; set; }



        // public string ImporteMora { get; set; }
        //public string ImporteMontoMinimo { get; set; }
    }
}
