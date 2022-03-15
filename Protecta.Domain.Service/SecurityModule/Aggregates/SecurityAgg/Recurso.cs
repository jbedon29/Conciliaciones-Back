using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.SecurityModule.Aggregates.SecurityAgg
{
    public class ListaRecursoRespuesta
    {
        public int IdRecurso { get; set; }
        public string Flag { get; set; }
        public string Modulo { get; set; }
        public string Opcion { get; set; }
        public string Descripcion { get; set; }

    }
}