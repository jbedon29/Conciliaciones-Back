using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class DatosConsultaFormaPago
    {
        public string doc_contratante;
        public string rec_nc;
        public int tipo;

    } // lo carga en tb temporal 

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
        public List<ConsultaInsertListaFPTemp> ConsultaInsertListaFPTemp = new List<ConsultaInsertListaFPTemp>();
    }
    public class ConsultaInsertListaFPTemp
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
        public int Tipo_forma_pago { get; set; }


    }
}
