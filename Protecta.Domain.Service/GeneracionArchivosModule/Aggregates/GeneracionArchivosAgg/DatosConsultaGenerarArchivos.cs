using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.GeneracionArchivosModule.Aggregates.GeneracionArchivosAgg
{
    public class DatosConsultaGenerarArchivos
    {
        public Int32 idProducto { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string fechaGeneracion { get; set; }
        public string id_planillas { get; set; }
    }
}
