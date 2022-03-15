using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Reporte
{
    public class ReportePlanillaPendienteDto
    {
        public string IdPlanilla { get; set; }
        /*
        public string Descripcion
        {
            get;
            set;
        }*/

        public decimal Monto { get; set; }

        public string FechaPlanilla { get; set; }

        public string FechaProceso { get; set; }

        public string NumeroOperacion { get; set; }
        /*
        public int IdTipoMedioPago { get; set; }
        */
        public string DescripcionMedioPago { get; set; }

        //INI JF
        public decimal saldo { get; set; }

        public string fechaAbono { get; set; }

        public decimal comisionDirecta { get; set; }

        public decimal comisionIndirecta { get; set; }

        public decimal comisionTotal { get; set; }

        public string IdProducto { get; set; }

        public string CodigoCanal { get; set; }

        public string DescripcionCanal { get; set; }


        //FIN JF

        public string moneda { get; set; }

    }
}
