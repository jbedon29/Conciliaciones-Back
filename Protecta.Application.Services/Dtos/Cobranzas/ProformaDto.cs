using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ProformaDto
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
        // public string ImporteMora { get; set; }
        // public string ImporteMontoMinimo { get; set; }
        // add for pA
        public string Ramo { get; set; }
        public string Producto { get; set; }

    }
    public class PlanillaDto
    {
        public bool Resultado { get; set; } = true;
        public string MensajeError { get; set; }
        public string RutaTrama { get; set; }
     
        public List<ProformaDto> ListaProforma { get; set; }
    }
}
