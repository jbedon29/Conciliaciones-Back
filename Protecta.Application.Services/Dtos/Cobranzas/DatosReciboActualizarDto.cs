using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    [DataContract]
    [Serializable]
    public class DatosReciboActualizarDto
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
