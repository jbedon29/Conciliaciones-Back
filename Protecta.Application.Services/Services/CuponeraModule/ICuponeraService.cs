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
        Task<GenerateResponse> ValidarPrintCupon(PrintCupon paramPrint);


        Task<GenerateResponse> ValidateCouponBook(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> ValidateCoupon(ParametersReciboDto parametersRecibo);

        Task<GenerateResponse> RecalculateCouponBook(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> RefactorCoupon(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> ValCantCoupons(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> ValCouponbook(ParametersReciboDto parametersRecibo);
        Task<List<Ramo>> ListRamos();
        Task<List<Producto>> ListProductos(ParametersReciboDto parametersRecibo);
        Task<GenerateResponse> ValPolFact(ParametersReciboDto parametersRecibo);
        Task<List<Recibo>> ListRecibos(ParametersReciboDto parametersRecibo);
    }
}
