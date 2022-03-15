using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    [DataContract]
    [Serializable]
    public class DatosReciboCobroDto
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
