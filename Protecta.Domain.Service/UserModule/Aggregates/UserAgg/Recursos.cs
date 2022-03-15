using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.UserModule.Aggregates.UserAgg
{
    public class Recursos
    {
        public int IdRecursos { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Descripcion { get; set; }
        public string BadgeType { get; set; }
        public string BadgeValue { get; set; }
        public int IdRecursoPadre { get; set; }
        public int IdSistema { get; set; }
        public int Orden { get; set; }
        public int Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
