using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg
{
    public class Mensaje
    {
        public string IdMensaje { get; set; }
        public string DescripcionMensaje { get; set; }
        public string TipoMensaje { get; set; }
    }
}
