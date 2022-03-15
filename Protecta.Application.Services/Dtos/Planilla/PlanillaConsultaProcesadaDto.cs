using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Planilla
{
    public class PlanillaConsultaProcesadaDto
    {
        //Control de cambio 1.1
        public string CodigoCanal { get; set; }

        public string DescripcionCanal { get; set; }

        public string IdPlanilla { get; set; }

        public string FechaPlanilla { get; set; }

        public string TotalPlanilla { get; set; }

        public string NumeroOperacion { get; set; }

        //public string IdDeposito { get; set; }

        public string FechaDeposito { get; set; }

        public string TotalDeposito { get; set; }
        public string SaldoDeposito { get; set; }
        public string ImporteDeposito { get; set; }

        public string Usuario { get; set; }

        #region Conciliaciones 1.3.1.8.5

        public string EstadoPlanilla { get; set; }
        public string IdEstadoPlanilla { get; set; }

        #endregion     
        public string FechaConciliacion { get; set; }
        public string Banco { get; set; }
        public string comision { get; set; }
        public string MedioDePago { get; set; }
        public string moneda { get; set; }
    }
}

