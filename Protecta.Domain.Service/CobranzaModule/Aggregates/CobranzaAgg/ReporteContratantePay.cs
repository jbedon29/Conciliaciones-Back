using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ReporteContratantePay
    {
        //sclient
        public string contratante { get; set; }

        //SCLIENTNAME
        public string nom_contratante { get; set; }
        public string cant_polizas { get; set; }
    }
}
