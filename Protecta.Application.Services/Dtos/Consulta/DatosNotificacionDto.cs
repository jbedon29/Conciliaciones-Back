using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Consulta
{
    public class DatosNotificacionDto
    {
        public int IndPlanilla
        {
            get;
            set;
        }

        public string Planilla
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
