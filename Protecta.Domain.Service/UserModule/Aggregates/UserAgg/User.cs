using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.UserModule.Aggregates.UserAgg
{
    public class User
    {
        public Int64 Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
    }
}