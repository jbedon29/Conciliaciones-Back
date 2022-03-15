using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ConciliacionDto
    {
        public bool IsValido { get; set; }
        public string Mensaje { get; set; }
        public string NumeroRecibo { get; set; }
        public string Importe { get; set; }
        public string FechaPago { get; set; }
        public string IdMoneda { get; set; }
        public string FlagExtorno { get; set; }
        public string NombreCliente { get; set; }
        public string NumeroDocuento { get; set; }
        public string FechaVencimiento { get; set; }
        public string NumeroCuenta { get; set; }
        public string FechaOperacion { get; set; }
        public string NumeroOperacion { get; set; }
        public string Referencia { get; set; }
        public int IdBanco { get; set; }
        public int IdProducto { get; set; }
        public string IdProceso { get; set; }
        public string UserCode { get; set; }
        public string MontoTotalOrigen { get; set; }
        public string ImporteOrigen { get; set; }

    }
    public class ListadoConciliacionDto
    {
        public bool IsValido { get; set; }
        public string Mensaje { get; set; }
        public string IdProceso { get; set; }
        public string IdBanco { get; set; }
        public string TipoOperacion { get; set; }
        public string IdProducto { get; set; }
        public string NumeroRecibo { get; set; }
        public string Importe { get; set; }
        public string IdMoneda { get; set; }
        public string MontoFormaPago { get; set; } 
        public string IdTipoPago { get; set; }
        public string IdCuentaBanco { get; set; }
        public string NumeroOperacion { get; set; }
        public string FechaOperacion { get; set; }
        public string Referencia { get; set; }
        public string UserCode { get; set; }   
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public string FechaCarga { get; set; }
        public string FechaVencimiento { get; set; }
        public int FlagExtorno { get; set; }
        public string FechaCargaArchivo { get; set; }


    }
    
}
