using System;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosConsultaPlanilla
    {
        public int IdProducto
        {
            get;
            set;
        }

        public string FechaDesde
        {
            get;
            set;
        }

        public string FechaHasta
        {
            get;
            set;
        }
        //Agregado 16/07/2018
        public string Usuario
        {
            get;
            set;
        }
        //Control de Cambio 1.1
        public long IdCanal
        {
            get;
            set;
        }

        public long IdBanco
        {
            get;
            set;
        }

        public long IdCuenta
        {
            get;
            set;
        }

        public string IdPlanilla
        {
            get;
            set;
        }
        public long IdTipoCobro
        {
            get;
            set;
        }
    }
}
