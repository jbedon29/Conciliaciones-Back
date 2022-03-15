using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    [DataContract]
    [Serializable]
    public class DatosConsultaReciboCobro
    {
        [DataMember]
        public List<Detalles> detalles { get; set; }
        [DataMember]
        public int nUserCode { get; set; }
        [DataMember]
        public long idProductoPay { get; set; }
        [DataMember]
        public long idRamo { get; set; }
    }

    public class Detalles
    {
        [DataMember]
        public long nroPoliza { get; set; }
        [DataMember]
        public long nroRecibo { get; set; }
    }
}