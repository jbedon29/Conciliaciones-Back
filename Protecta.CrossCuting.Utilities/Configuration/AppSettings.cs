using System;

namespace Protecta.CrossCuting.Utilities.Configuration
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionStringORA { get; set; }
        public string ConnectionStringTimeP { get; set; }
        public string ConnectionStringConciliacion { get; set; }

        public enum EstadoPlanilla
        {
            Ingresado = 1101,
            ConciliadoTotal = 1103,
            ConciliadoParcial = 1102,
            EnviadoExactus = 1104,
            EnviadoComprobante = 1105
        }
    }
}

