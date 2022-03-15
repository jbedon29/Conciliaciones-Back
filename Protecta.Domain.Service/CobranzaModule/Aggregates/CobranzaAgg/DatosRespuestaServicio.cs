using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    [DataContract]
    [Serializable]
    public class DatosRespuestaServicio
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public long idPlanilla { get; set; }
        [DataMember]
        public string message { get; set; }
    }

 
}