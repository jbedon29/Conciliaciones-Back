using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class TramaDto
    {
        public bool EsValido { get; set; }
        public string Mensaje { get; set; }
        public string StringTrama { get; set; }
        public string Fila { get; set; }
        public int    IdBanco { get; set; }
        public int    IdProducto { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int Segmento { get; set; }
        public string TipoIngreso { get; set; }
        public string TiempoTranscurrido { get; set; }
        public int CodigoUsuario { get; set; }
       
        public List<ListadoConciliacionDto> listado { get; set; }
        
    }
}
