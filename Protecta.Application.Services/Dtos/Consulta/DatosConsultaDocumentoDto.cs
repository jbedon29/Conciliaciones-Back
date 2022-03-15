using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosConsultaDocumentoDto
    {
        public string NumeroFactura { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
    }
}