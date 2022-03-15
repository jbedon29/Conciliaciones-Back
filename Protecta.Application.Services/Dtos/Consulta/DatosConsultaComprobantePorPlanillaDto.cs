using System;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosConsultaComprobantePorPlanillaDto
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

    }
}
