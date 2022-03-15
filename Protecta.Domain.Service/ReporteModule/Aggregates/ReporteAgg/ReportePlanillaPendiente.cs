using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg
{
    public class ReportePlanillaPendiente
    {
        public string IdPlanilla { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string FechaPlanilla { get; set; }
        public string FechaProceso { get; set; }
        public string NumeroOperacion { get; set; }
        public string IdProducto { get; set; }
        public string IdTipoMedioPago { get; set; }
        public string DescripcionMedioPago { get; set; }
        public string CodigoCanal { get; set; }
        public string DescripcionCanal { get; set; }
       

        //INI JF
        public decimal saldo { get; set; }
        public string fechaAbono { get; set; }
        public decimal comisionDirecta { get; set; }
        public decimal comisionIndirecta { get; set; }
        public decimal comisionTotal { get; set; }
        //FIN JF
        public string moneda { get; set; }

    }
}
