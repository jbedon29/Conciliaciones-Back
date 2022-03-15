using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class Loadtransaction
    {
        public string FileName { get; set; }
        public int IdLoadTransaction { get; set; }
        public int idArchivo { get; set; }
    }
}
