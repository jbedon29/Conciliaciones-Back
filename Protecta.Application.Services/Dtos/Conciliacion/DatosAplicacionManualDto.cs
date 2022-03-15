using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Conciliacion
{
    public class DatosAplicacionManualDto
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

        public string IdPlanillas { get; set; }

        public string IdDepositos { get; set; }

        public string Usuario { get; set; }

    }
}
