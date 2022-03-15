using System;

namespace Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg
{
    public class DatosConsultaArchivos
    {
        public int IdTipoArchivo
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
    }
}
