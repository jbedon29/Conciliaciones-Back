using Protecta.Application.Service.Dtos.Cobranzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Models;
using Microsoft.AspNetCore.Http;

namespace Protecta.Application.Service.Services.CobranzasModule
{
    public interface ICobranzasService
    {
        Task<IEnumerable<BancoDto>> ListarBancos();
        Task<IEnumerable<CuentaDto>> ListarCuenta(int Idbanco);
        Task<TramaDto> ValidarTrama(string usercode, string base64String, int idbanco, int idproducto, string idproceso, string fechaInicio, string fechaFinal, string CodProforma);
        Task<PlanillaDto> ObtenerTrama(TramaDto trama);
        Task<ResponseControl> InsertarFacturaFormaPago(List<ListadoConciliacionDto> listadoConciliacionDtos);
        Task<ResponseControl> Validar_Planilla_FacturaAsync(ListadoConciliacionDto Listado);
        Task<ResponseControl> ObtenerFormaPago(int idbanco,string idproceso);
        Task<IEnumerable<Tipo_PagoDto>> ListarTipoPago();

        Task<ResponseControl> ProcessConciliaciones(IFormFile file, int IdTransc);
        Task<ResponseControl> ProcessDelCargaDeposito(IFormFile file,int idArchivo, int IdTransc);
        Task<IEnumerable<RamoPayDto>> ListarRamoPay();
        Task<IEnumerable<EstadoReciboDto>> ListarEstadoRecibo();
        Task<IEnumerable<EstadoEnvioDto>> ListarEstadoEnvioCE();


        Task<IEnumerable<DatosRespuestaProductoPayDto>> ListarProductoPay(DatosConsultaProductoPayDto datosConsultaProductoPayDto);
        Task<ReporteContratantePayDto> ListarContratantePay(DatosConsultaContratantePayDto datosConsultaContratantePay);
        Task<IEnumerable<ReporteReciboPayDto>> ListarReciboPay(DatosConsultaReciboPayDto datosConsultaReciboPay);

        Task<IEnumerable<ReporteEnvioPayDto>> ListarEnvioPay(DatosConsultaEnvioPayDto datosConsultaEnvioPay);

        Task<IEnumerable<ReporteReciboDetallePayDto>> ListarReciboDetalle(DatosConsultaReciboDetalleDto datosConsultaEnvioPay);

        Task<ResponseControl> ActualizarMotivos(DatosReciboActualizarDto datosReciboActualizarDto);

        Task<ResponseControl> CobroRecibo(DatosReciboCobroDto datosReciboCobroDto);
        Task<DatosRespuestaContratanteNCDto> ListarContratanteNC(DatosConsultaContratanteNCDto datosConsultaContratanteNCDto);
        Task<IEnumerable<ReporteReciboPendienteNCDto>> ListarReciboPendienteNC(DatosConsultaReciboPendienteNCDto datosConsultaReciboPendienteNCDto);
        
        Task<IEnumerable<ReporteFormaPagoDto>> ReporteFormaPago(DatosConsultaFormaPagoDto datosConsultaFormaPagoDto);
        
          Task<IEnumerable<TipoFPDto>> ListarTipoFP();



        //seguimiento de lote 09/11/2021

        Task<IEnumerable<ReporteSeguimientoLoteDto>> ReporteSeguimientoLote(DatosConsultaSeguimientoLoteDto datosConsultaSeguimientoLoteDto);


        //Task<ResponseControl> ProcessGenerarPlanillaNC(DatosGenerarPlanillaNCDto datosGenerarPlanillaNCDto);







    }
}

