using System;

namespace Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg
{
    public class NotaCredito
    {
        public int IdNotaCredito { get; set; }
        public int IdProducto { get; set; }
        public string NumeroNotaCredito { get; set; }
        public int IdFacturaDeposito { get; set; }
        public decimal DcMonto { get; set; }
        public string VcUsuarioCreacion { get; set; }
        public string DtFechaCreacion { get; set; }
        public string VcUsuarioModificacion { get; set; }
        public string DtFechaModificacion { get; set; }
        public int IddgEstado { get; set; }
    }
}
