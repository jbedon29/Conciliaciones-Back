using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class Conciliacion
    {
        public bool IsValido { get; set; }
        public string Mensaje { get; set; }
        public string NumeroRecibo { get; set; }
        public string Importe { get; set; }
        public string FechaPago { get; set; }
        public string IdMoneda { get; set; }
        public string FlagExtorno { get; set; } 
        public string NombreCliente { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaOperacion { get; set; }
        public string NumeroCuenta { get; set; }        
        public int IdBanco { get; set; }
        public int IdProducto { get; set; }
        public string NumeroDocuento { get; set; }
        public string NumeroOperacion { get; set;}
        public string Referencia { get; set; }
        public string IdProceso { get; set; }
        public string MontoTotal { get; set; }
        public string CantTotal { get; set; }
        public string UserCode { get; set; } = string.Empty;
        public string MontoTotalOrigen { get; set; }
        public string ImporteOrigen { get; set; }


    }



}
