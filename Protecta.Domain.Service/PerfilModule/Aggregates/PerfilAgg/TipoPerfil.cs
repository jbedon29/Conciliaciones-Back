using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg
{
    public class TipoPerfil
    {
        public int IdTipoPerfil
        {
            get; set;
        }


        public string VcDescripcion
        {
            get;
            set;
        }

        public int IddgEstado
        {
            get;
            set;
        }

        public string DtFechacreacion
        {
            get;
            set;
        }

        public string VcUsuariocreacion
        {
            get;
            set;
        }

        public string DtFechamodificacion
        {
            get;
            set;
        }

        public string VcUsuariomodificacion
        {
            get;
            set;
        }
    }
}