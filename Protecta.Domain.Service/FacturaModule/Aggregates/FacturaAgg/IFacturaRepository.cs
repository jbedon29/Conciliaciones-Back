using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg;
using Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg;

namespace Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg
{
    public interface IFacturaRepository
    {
        Task<List<DocumentoAbono>> ListarDocumentosAbonos(DatosConsultaDocumento documento);

        Task<FacturaDeposito> ValidarExisteFacturaDeposito();

        Task<FacturaDeposito> ObtenerFacturaDeposito(DatosFacturaAbonos datosFacturaAbonos);

        Task<string> RegistrarFacturaDeposito(FacturaDeposito factura_deposito);

        Task<string> ActualizarEstadoDeposito(DetalleFacturaDeposito detalle_factura_deposito);

        List<Deposito> ListarDepositoNoConciliado();

        string ObtenerNumeroSerie();

        Task<List<FacturaDeposito>> ListarFacturaDeposito();

        Task<string> RegistrarNotaCredito(NotaCredito nota_credito);

        Task<string> AnularFacturaDeposito(FacturaDeposito factura_deposito);
    }
}
