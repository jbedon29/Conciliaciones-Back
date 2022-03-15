using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.GeneracionArchivos
{
    public class DatosConsultaGeneracionArchivoDto
    {
        public Int32 idProducto { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string fechaGeneracion { get; set; }
        public string id_planillas { get; set; }
    }
}
