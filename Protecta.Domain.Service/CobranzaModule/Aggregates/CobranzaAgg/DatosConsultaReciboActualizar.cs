using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    [DataContract]
    [Serializable]
    public class DatosConsultaReciboActualizar
    {
        [DataMember]
        //public List<long> nrosEnvio { get; set; }
        public long nrosEnvio { get; set; }

        [DataMember]
        public string sMotivo { get; set; }
        [DataMember]
        public int nUserCode { get; set; }
    }
}
