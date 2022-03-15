using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg
{
    public class Perfil
    {
        public int IdPerfil
        {
            get;
            set;
        }

        public string TipoPerfil
        {
            get;
            set;
        }

        public string VcNombrePerfil
        {
            get;
            set;
        }

        public string VcDescripcion
        {
            get;
            set;
        }

        public int Estado
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