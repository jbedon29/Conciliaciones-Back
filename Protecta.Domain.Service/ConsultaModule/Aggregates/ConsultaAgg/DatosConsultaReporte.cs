﻿using System;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosConsultaReporte
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
    }
}
