using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    //Este metodo es para el proyecto de Nota de crédito 28/09/2021

    public class DatosConsultaFormaPagoDto
    {
        public string doc_contratante;
        public string rec_nc;
        public int tipo;
        public int tipoMoneda;

    }
    // lo carga en tb temporal 

    public class ListaAgregadaFPTempDto
    {
        public string Documento { get; set; }
        public string Contratante { get; set; }
        public string Fecha_ope { get; set; }
        public string Moneda { get; set; }
        public string Monto { get; set; }
        public string Tipo { get; set; }
        public string Recibo { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        //public string Usercode { get; set; }

    }
    public class ObjParameter
    {
        [DataMember]
        public List<ConsultaInsertListaFPTempDto> ConsultaInsertListaFPTemp = new List<ConsultaInsertListaFPTempDto>();
    }
    public class ConsultaInsertListaFPTempDto
    {
        public string nUserCode { get; set; }
        public string Documento { get; set; }
        public string Contratante { get; set; }
        public string Fecha_ope { get; set; }
        public string Moneda { get; set; }
        public string Monto { get; set; }
        public string Tipo { get; set; }
        public string Recibo { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string Referencia { get; set; }

        public int TipoFormaPago { get; set; }

    }
}
