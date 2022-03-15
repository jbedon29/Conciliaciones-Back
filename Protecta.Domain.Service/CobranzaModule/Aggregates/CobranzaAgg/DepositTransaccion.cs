using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public class DepositTransaccion
    {
        public string numOperaccion { get; set; }
        public string FechaOperacion { get; set; }
        public string monto { get; set; }
        public string usuario { get; set; }
    }

    public class DepositTransaccionVisa
    {
        public string nombre_archivo { get; set; }
        public string ruc { get; set; }
        public string razon_social { get; set; }
        public string cod_comercio { get; set; }
        public string nombre_comercial { get; set; }
        public string fecha_operacion { get; set; }
        public string fecha_deposito { get; set; }
        public string producto { get; set; }
        public string tipo_operacion { get; set; }
        public string tarjeta { get; set; }
        public string origen_tarjeta { get; set; }
        public string tipo_tarjeta { get; set; }
        public string marco_tarjeta { get; set; }
        public string moneda { get; set; }
        public string importe_operacion { get; set; }
        public string dcc { get; set; }
        public string monto_dcc { get; set; }
        public string comision_total { get; set; }
        public string comision_niubiz { get; set; }
        public string igv { get; set; }
        public string suma_depositada { get; set; }
        public string estado { get; set; }
        public string id_operacion { get; set; }
        public string cuenta_banco_pagador{ get; set; }
        public string banco_pagador { get; set; }
        public string tipo_transac { get; set; }
        public string cip { get; set; }
        public string fecha_creacion_cip { get; set; }
        public string vc_usuario_creacion { get; set; }
        //
        public string usuario { get; set; }




    }
}

