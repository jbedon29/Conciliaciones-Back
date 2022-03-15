using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class Trama
    {
        public string StringTrama { get; set; }
        public string Fila { get; set; }
        public int IdBanco { get; set; }
        public int Segmento { get; set; }
        public string TipoIngreso { get; set; }
        public int IdProducto { get; set; }
        public string FechaInicial { get; set; } 
        public string FechaFinal { get; set; }
        public int CodigoUsuario { get; set; }
    }
}
