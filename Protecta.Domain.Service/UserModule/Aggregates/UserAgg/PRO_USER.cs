using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.UserModule.Aggregates.UserAgg
{
    public class PRO_USER
    {
        public int ID_USUARIO { get; set; }
        public string VC_COD_USUARIO { get; set; }
        public string VC_APE_PATERNO { get; set; }
        public string VC_APE_MATERNO { get; set; }
        public string VC_NOMBRE_USUARIO { get; set; }
        public string VC_CORREO_USUARIO { get; set; }
        public int N_ESTADO { get; set; }
        public string VC_USUARIO_CREACION { get; set; }
        public DateTime DT_FECHA_CREACION { get; set; }
        public string VC_USUARIO_MODIFICACION { get; set; }
        public DateTime DT_FECHA_MODIFICACION { get; set; }
    }
}