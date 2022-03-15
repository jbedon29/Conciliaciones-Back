using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.CrossCuting.Utilities.Configuration
{
    public class Ldap
    {
        public string Url { get; set; }
        public string SearchBase { get; set; }
        public string BindDn { get; set; }
        public string BindCredentials { get; set; }
        public string SearchFilter { get; set; }
        public string AdminCn { get; set; }
        public string UrlFE { get; set; }

        public string opcion { get; set; }
        public string usernameServidor { get; set; }
        public string Dominio { get; set; }
        public string PasswordServidor { get; set; }
        public string UrlServicioGestor { get; set; }
        public string RutaFTP { get; set; }
    }
}