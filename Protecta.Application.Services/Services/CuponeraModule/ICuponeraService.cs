using Protecta.Application.Service.Dtos.Cuponera;
using Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.CuponeraModule
{
    public interface ICuponeraService
    {
        Task<IEnumerable<TransacionDto>> ListarTransaciones();
        Task<ReciboDto> GetInfoRecibo(ParametersReciboDto parameters);
        Task<List<CuponDto>> GetInfoCuponPreview(ParametersReciboDto parameters);
        Task<GenerateResponse> GenerateCupon(ParametersReciboDto parametersRecibo);
        Task<ReciboDto> GetInfoCuponera(ParametersReciboDto parametersRecibo);
        Task<List<CuponDto>> GetInfoCuponeraDetail(ParametersReciboDto parameters);



        Task<DetalleReciboDto> GetInfoMovimiento(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> AnnulmentCupon(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> PrintCupon(PrintCupon paramPrint);
    }
}
