using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg
{
    public class RecursoProceso
    {
        public int IdTipoPerfil { get; set; }
        public string VcNombrePerfil { get; set; }
        public string VcDescripcion { get; set; }
        public int IdPerfil { get; set; }
        public string IdRecursos { get; set; }
        public string VcUsuariocreacion { get; set; }
    }
}