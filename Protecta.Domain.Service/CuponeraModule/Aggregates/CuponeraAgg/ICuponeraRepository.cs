using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
   public interface ICuponeraRepository
    {
        Task<List<Transacion>> ListarTransaciones();
        Task<GenerateResponse> ValidateRecibo(ParametersRecibo parametersRecibo);
        Task<Recibo> GetInfoRecibo(ParametersRecibo parameters);
        Task<List<Cupon>> GetInfoCuponPreview(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> GenerateCupon(ParametersRecibo parametersRecibo);
        Task<Recibo> GetInfoCuponera(ParametersRecibo parametersRecibo);
        Task<List<Cupon>> GetInfoCuponeraDetail(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> ValidarPrintCupon(PrintCupon paramPrint);
        Task<List<TemplateCupon1>> PrintCupon(PrintCupon paramPrint);
        Task<List<TemplateCupon2>> PrintCuponCrono(PrintCupon paramPrint);


        Task<Recibo> GetInfoCupon(ParametersRecibo parametersRecibo);
        Task<DetalleRecibo> GetInfoMovimiento(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> AnnulmentCupon(ParametersRecibo parametersRecibo);
        


        Task<GenerateResponse> ValidateCouponBook(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> ValidateCoupon(ParametersRecibo parametersRecibo);

        Task<GenerateResponse> RecalculateCouponBook(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> RefactorCoupon(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> ValCantCoupons(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> ValCouponbook(ParametersRecibo parametersRecibo);

        Task<List<Ramo>> ListRamos();
        Task<List<Producto>> ListProductos(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> ValPolFact(ParametersRecibo parametersRecibo);
        Task<List<Recibo>> ListRecibo(ParametersRecibo parametersRecibo);
    }
}
