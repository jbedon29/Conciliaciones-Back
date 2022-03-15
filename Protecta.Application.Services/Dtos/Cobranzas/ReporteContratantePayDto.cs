using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class ReporteContratantePayDto
    {
        //sclient
        public string contratante { get; set; }

        //SCLIENTNAME
        public string nom_contratante { get; set; }
        public string cant_polizas { get; set; }

    }
}
