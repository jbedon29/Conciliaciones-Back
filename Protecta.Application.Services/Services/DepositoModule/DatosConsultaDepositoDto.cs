namespace Protecta.Application.Service.Services.DepositoModule
{
    public class DatosConsultaDepositoDto
    {
        public long IdBanco { get; set; }

        public long IdCuenta{ get; set; }

        public long IdMoneda { get; set; }

        public long IdProducto { get; set; }

        public string FechaDesde { get; set; }

        public string FechaHasta { get; set; }
    }
}