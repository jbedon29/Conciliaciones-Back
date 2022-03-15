using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Perfil
{
    public class PerfilDto
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