using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Perfil
{
    public class RecursoProcesoDto
    {
        public int IdTipoPerfil { get; set; }
        public string VcNombrePerfil { get; set; }
        public string VcDescripcion { get; set; }
        public int IdPerfil { get; set; }
        public string IdRecursos { get; set; }
        public string VcUsuario { get; set; }
        public string VcUsuariocreacion { get; set; }
    }
}