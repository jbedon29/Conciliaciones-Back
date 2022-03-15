using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Conciliacion
{
    public class ConciliacionDetalleDto
    {
        public int IdConciliacionplanilla
        {
            get;
            set;
        }

        public int IdDetalleplanilla
        {
            get;
            set;
        }

        public int IdDeposito
        {
            get;
            set;
        }

        public string DtFechaconciliacion
        {
            get;
            set;
        }

        public int iddgEstado
        {
            get;
            set;
        }

        public string DCmontopagado
        {
            get;
            set;
        }
    }
}
