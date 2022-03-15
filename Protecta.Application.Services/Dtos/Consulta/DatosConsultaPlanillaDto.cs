using System;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosConsultaPlanillaDto
    {
        public long IdProducto
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
