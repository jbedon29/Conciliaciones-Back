using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosAplicaConciliacionDto
    {
        public long IdProducto
        {
            get;
            set;
        }

        public string FechaDesde
        {
            get;
            set;
        }

        public string FechaHasta
        {
            get;
            set;
        }

        public string Usuario { get; set; }

        public long IdTipoCobro
        {
            get;
            set;
        }
        
    }
}
