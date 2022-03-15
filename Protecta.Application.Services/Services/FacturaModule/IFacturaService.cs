using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.Factura;

namespace Protecta.Application.Service.Services.FacturaModule
{
    public interface IFacturaService
    {
        Task<IEnumerable<DocumentoAbonoDto>> ListarDocumentosAbonos(DatosConsultaDocumentoDto datosConsultaDocumentoDto);

        Task<string> ValidarExisteFacturaDeposito();

        Task<string> GenerarFacturaAbonos(DatosFacturaAbonosDto datosFacturaAbonos);

        Task<string> GenerarNotaCredito();
    }
}
