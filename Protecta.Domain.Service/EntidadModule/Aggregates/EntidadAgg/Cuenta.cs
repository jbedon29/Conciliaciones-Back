using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.EntidadModule.Aggregates.EntidadAgg
{
    public class Cuenta
    {
        public long ID_CUENTA
        {
            get;
            set;
        }

        public long ID_ENTIDAD
        {
            get;
            set;
        }

        public string NUMERO_CUENTA
        {
            get;
            set;
        }

        public long ID_MONEDA
        {
            get;
            set;
        }


        public string CODIGO_MONEDA
        {
            get; set;
        }
    }
}
