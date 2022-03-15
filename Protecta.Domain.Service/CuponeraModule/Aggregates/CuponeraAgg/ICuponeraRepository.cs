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
        Task<List<TemplateCupon1>> PrintCupon(PrintCupon paramPrint);




        Task<Recibo> GetInfoCupon(ParametersRecibo parametersRecibo);
        Task<DetalleRecibo> GetInfoMovimiento(ParametersRecibo parametersRecibo);
        Task<GenerateResponse> AnnulmentCupon(ParametersRecibo parametersRecibo);
        



    }
}
