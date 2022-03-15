using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg
{
    public class COMM_VISANET
    {
        public string SCERTYPE { get; set; }
        public int NBRANCH { get; set; }
        public int NPRODUCT { get; set; }
        public long NPOLICY { get; set; }
        public int NCOMERCIO { get; set; }
        public string NOMBRE_COMERCIAL { get; set; }
        public DateTime DEFFECDATE { get; set; }
        public DateTime FEC_TRANSACCION { get; set; }
        public DateTime FEC_PROCESO { get; set; }
        public string TIPO_OPERACION { get; set; }
        public string DESC_COD_CONTABLE { get; set; }
        public string NUM_TARJETA { get; set; }
        public string ORI_TARJETA { get; set; }
        public string TIPO_TARJETA { get; set; }
        public string TIPO_CAPTURA { get; set; }
        public string MONEDA { get; set; }
        public decimal IMPORTE_TRANSAC { get; set; }
        public decimal IMPORTE_CASHBACK { get; set; }
        public decimal IMPORTE_COMISION_TOTAL { get; set; }
        public decimal IMPORTE_COMISION_GRAVABLE { get; set; }
        public decimal NPORCENT_COMISION { get; set; }
        public decimal IMPORTE_IGV { get; set; }
        public decimal IMPORTE_ABONAR { get; set; }
        public string ESTADO { get; set; }
        public string STYP_COMM { get; set; }
        public DateTime FEC_ABONO { get; set; }
        public string AUTORIZACION { get; set; }
        public string ID_UNICO { get; set; }
        public string NUM_TERMINAL { get; set; }
        public int NLOTE { get; set; }
        public string NUM_REFERENCIA { get; set; }
        public int NUM_CUOTAS { get; set; }
        public string CUENTA_BANCARIA { get; set; }
        public int NBANK_CODE { get; set; }
        public string TRANSAC_CUOTAS { get; set; }
        public string NOMBRE_PROGRAMA { get; set; }
        public decimal IMPORTE_DESCONT { get; set; }
        public int NOPERACION_BANCO { get; set; }
        public long NRECEIPT { get; set; }
        public DateTime DCOMPDATE { get; set; }
        public int NUSERCODE { get; set; }
    }
}
