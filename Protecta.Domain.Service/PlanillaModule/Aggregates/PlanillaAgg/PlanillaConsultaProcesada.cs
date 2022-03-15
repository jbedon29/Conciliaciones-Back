using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg
{
    public class PlanillaConsultaProcesada
    {
        //Control de cambio 1.1
        public string CodigoCanal
        {
            get;
            set;
        }

        public string DescripcionCanal
        {
            get;
            set;
        }

        public long IdPlanilla
        {
            get;
            set;
        }

        public DateTime FechaPlanilla
        {
            get;
            set;
        }

        public decimal TotalPlanilla
        {
            get;
            set;
        }

       
        public string NumeroOperacion
        {
            get;
            set;
        }

        //public long IdDeposito
        //{
        //    get;
        //    set;
        //}
      
        public DateTime FechaDeposito
        {
            get;
            set;
        }

        public decimal TotalDeposito
        {
            get;
            set;
        }

        public decimal SaldoDeposito
        {
            get;
            set;
        }

        public decimal ImporteDeposito
        {
            get;
            set;
        }

        public string Usuario
        {
            get;
            set;
        }

        public DateTime FechaConciliacion
        {
            get;
            set;
        }

        public string Banco
        {
            get;
            set;
        }

        public decimal comision
        {
            get;
            set;
        }

        public string MedioDePago
        {
            get;
            set;
        }
        public string moneda
        {
            get;
            set;
        }

        #region Conciliaciones 1.3.1.8.5

        public string EstadoPlanilla { get; set; }
        public string IdEstadoPlanilla { get; set; }
        #endregion
    }
}
