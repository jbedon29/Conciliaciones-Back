using System;

namespace Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg
{
    public class DetalleFacturaDeposito
    {
        public int IdDetalleFacturaDeposito { get; set; }
        public int IdFacturaDeposito { get; set; }
        public long IdDeposito { get; set; }
        public decimal Monto { get; set; }
        public int IdTipoMedioPago { get; set; }
        public string VcUsuarioCreacion { get; set; }
        public string DtFechaCreacion { get; set; }
        public string VcUsuarioModificacion { get; set; }
        public string DtFechaModificacion { get; set; }
        public int IddgEstado { get; set; }
        public int IddgEstadoDeposito { get; set; }
    }
}