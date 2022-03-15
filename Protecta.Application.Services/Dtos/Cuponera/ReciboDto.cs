using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cuponera
{
    public class ReciboDto
    {
        public string ClientName { get; set; }
        public string SClient { get; set; }
        public string Fecha { get; set; }
        public string NroRecibo { get; set; }
        public string IdRamo { get; set; }
        public string Ramo { get; set; }
        public string IdProducto { get; set; }
        public string Producto { get; set; }
        public string NroPoliza { get; set; }
        public string NroCertificado { get; set; }
        public string Moneda { get; set; }
        public string InicioVigencia { get; set; }
        public string FinVigencia { get; set; }
        public string CantCupones { get; set; }
        public string MontoPrima { get; set; }
        public string MontoInicial { get; set; }
        public string MontoCupon { get; set; }
        public string PorcentajeInteres { get; set; }
        public string FechaPago { get; set; }
        public string TotalFinanciado { get; set; }
        public List<CuponDto> ListCupones { get; set; }
        public int P_NCODE { get; set; } = 0;
        public string P_SMESSAGE { get; set; }
    }
}
