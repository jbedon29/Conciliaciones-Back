using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosConsultaArchivosDto
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
