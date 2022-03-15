using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.UserModule.Aggregates.UserAgg
{
    public class Usuario
    {
        public Int64 Id { get; set; }
        public string CodUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CorreoUsuario { get; set; }
        public int Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
