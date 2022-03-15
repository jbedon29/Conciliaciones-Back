using System;
using System.Collections.Generic;

namespace Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg
{
    public class FacturaDeposito
    {
        public int IdFacturaDeposito { get; set; }
        public int IdProducto { get; set; }        
        public string NumeroFactura { get; set; }
        public decimal MontoTotal { get; set; }
        public string VcUsuarioCreacion { get; set; }
        public string DtFechaCreacion { get; set; }
        public string VcUsuarioModificacion { get; set; }
        public string DtFechaModificacion { get; set; }
        public int IddgEstado { get; set; }
        public string IndGeneracion { get; set; }
        public List<DetalleFacturaDeposito> DetalleFacturaDeposito { get; set; }
    }
}