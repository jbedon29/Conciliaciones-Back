using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class ReporteEnvioPay
    {
        public int nroEnvio { get; set; }
        public string sFechaEnvio { get; set; }
        public string sEstado { get; set; }
        public int nroRegistro { get; set; }
        public int nroCobro { get; set; }
        public int nroSinCobro { get; set; }
        public int nroError { get; set; }
        public string glosa { get; set; }
        public bool isCheck { get; set; }
        public int idEstado { get; set; }

        
    }
}
