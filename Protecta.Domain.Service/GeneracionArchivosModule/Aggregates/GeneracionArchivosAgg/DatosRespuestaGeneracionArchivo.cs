using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.GeneracionArchivosModule.Aggregates.GeneracionArchivosAgg
{
    public class DatosRespuestaGeneracionArchivo
    {
        public int id_planilla { get; set; }
        public string numeroOperacion { get; set; }
        public string fechaConciliacion { get; set; }
        public decimal montoBruto { get; set; }
        public decimal montoNeto { get; set; }
        public decimal comisionDirecta { get; set; }
        public decimal comisionIndirecta { get; set; }
        public string idDeposito { get; set; }
        public string idDepositoArchivo { get; set; }
        public decimal montoDeposito { get; set; }
        public decimal saldoDeposito { get; set; }
        //public string nombreArchivo { get; set; }
        public string fechaDeposito { get; set; }
        // No se encontraban en el Excel
        public string tipoMovimiento { get; set; }
        public string banco { get; set; }
        public string numeroCuenta { get; set; }
        public string id_dg_estado_planilla { get; set; }
    }
}
