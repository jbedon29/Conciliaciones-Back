using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Cobranzas;
using Protecta.Application.Service.Models;
using Protecta.Application.Service.Services.CobranzasModule;
using Protecta.CrossCuting.Log.Contracts;
using System.Web;
using Protecta.CrossCuting.Utilities.Configuration;
using Microsoft.AspNetCore.Http.Internal;

namespace Protecta.Application.Service.Controllers.Cobranzas
{
    [Route("api/[controller]")]
    public class CobranzaController : Controller
    {
        private ICobranzasService _cobranzaService;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CobranzaController));

        public CobranzaController(ILoggerManager logger,
            ICobranzasService _cobranzaService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            this._cobranzaService = _cobranzaService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarBancos()
        {
            _logger.LogInfo("Metodo Listar Bancos SCTR");

            var bancoResult = await _cobranzaService.ListarBancos();
            return Ok(bancoResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarTipoPago()
        {
            _logger.LogInfo("Metodo Listar Tipo Pago");

            var TipoPagoResult = await _cobranzaService.ListarTipoPago();
            return Ok(TipoPagoResult);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ListarCuentas(int idBanco)
        {
            _logger.LogInfo("Metodo Listar cuentas SCTR");

            var bancoResult = await _cobranzaService.ListarCuenta(idBanco);
            return Ok(bancoResult);
        }

        //LISTAR PAY

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarRamoPay()
        {
            _logger.LogInfo("Metodo Listar Ramo PAY");

            var ramoPayResult = await _cobranzaService.ListarRamoPay();
            return Ok(ramoPayResult);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarEstadoRecibo()
        {
            _logger.LogInfo("Metodo Listar ListarEstadoRecibo");

            var estadoReciboResult = await _cobranzaService.ListarEstadoRecibo();
            return Ok(estadoReciboResult);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarEstadoEnvioCE()
        {
            _logger.LogInfo("Metodo Listar ListarEstadoRecibo");

            var estadoEnvioResult = await _cobranzaService.ListarEstadoEnvioCE();
            return Ok(estadoEnvioResult);
        }

        //otro
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarProductoPay([FromBody] DatosConsultaProductoPayDto datosConsultaProductoPayDto)
        {
            _logger.LogInfo("Metodo Listar Producto PAY");

            var productoPayResult = await _cobranzaService.ListarProductoPay(datosConsultaProductoPayDto);
            return Ok(productoPayResult);
        }
        //fin otro


        [HttpPost("[action]")]
        public async Task<IActionResult> ListarContratantePay([FromBody]DatosConsultaContratantePayDto datosConsultaContratantePayDto)
        {
            _logger.LogInfo("Metodo Listar  ListarContratante");

            var result = await _cobranzaService.ListarContratantePay(datosConsultaContratantePayDto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarReciboPay([FromBody] DatosConsultaReciboPayDto datosConsultaReciboPayDto)
        {
            _logger.LogInfo("Metodo Listar ReciboPay");
            
            try
            {
                DateTime dfechaDesde = Convert.ToDateTime(datosConsultaReciboPayDto.FechaDesde);
                DateTime dfechaHasta = Convert.ToDateTime(datosConsultaReciboPayDto.FechaHasta);
                datosConsultaReciboPayDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
                datosConsultaReciboPayDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");
                var result  = await _cobranzaService.ListarReciboPay(datosConsultaReciboPayDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Metodo Listar  ListarReciboPay " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }
        
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarEnvioPay([FromBody] DatosConsultaEnvioPayDto datosConsultaEnvioPayDto)
        {
            _logger.LogInfo("Metodo Listar ReciboPay");

            try
            {
                DateTime dfechaDesde = Convert.ToDateTime(datosConsultaEnvioPayDto.FechaDesde);

                if(datosConsultaEnvioPayDto.FechaHasta != null)
                    {
                    DateTime dfechaHasta = Convert.ToDateTime(datosConsultaEnvioPayDto.FechaHasta);
                    datosConsultaEnvioPayDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");
                }
                datosConsultaEnvioPayDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
               
                var result = await _cobranzaService.ListarEnvioPay(datosConsultaEnvioPayDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Metodo Listar  ListarReciboPay " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarReciboDetalle([FromBody] DatosConsultaReciboDetalleDto datosConsultaReciboDetalleDto)
        {
            _logger.LogInfo("Metodo Listar ReciboPay");

            try
            {
                var result = await _cobranzaService.ListarReciboDetalle(datosConsultaReciboDetalleDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Metodo Listar  ListarReciboPay " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ActualizarMotivos([FromBody] DatosReciboActualizarDto datosReciboActualizarDto)
        {
            _logger.LogInfo("Metodo Listar ReciboPay");

            try
            {
                var result = await _cobranzaService.ActualizarMotivos(datosReciboActualizarDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Metodo Listar  ListarReciboPay " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CobroRecibo([FromBody] DatosReciboCobroDto datosReciboCobroDto)
        {
            _logger.LogInfo("Metodo Cobro Recibo");

            try
            {
                var result = await _cobranzaService.CobroRecibo(datosReciboCobroDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Metodo Cobro Recibo " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }



        [HttpPost("[action]")]
        public async Task<IActionResult> ValidarTrama([FromBody] Cargarlote cargarLoteJson)
        {
            try
            {
                var validarTramaResult = await _cobranzaService.ValidarTrama(cargarLoteJson.UserCode,
                                                                        cargarLoteJson.Data,
                                                                        cargarLoteJson.IdBanco,
                                                                        cargarLoteJson.IdProducto,
                                                                        cargarLoteJson.IdProceso,
                                                                        cargarLoteJson.FechaInicial,
                                                                        cargarLoteJson.FechaFinal,
                                                                        cargarLoteJson.CodProforma);
                return Ok(validarTramaResult);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                return Ok(ex.ToString());
            }

        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ObtenerTrama([FromBody] TramaDto trama)
        {
            try
            {
                var obtenerTramaResult = await _cobranzaService.ObtenerTrama(trama);
                return Ok(obtenerTramaResult);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                return Ok(ex);
            }
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarFacturaFormaPago([FromBody] List<ListadoConciliacionDto> listado)
        {

            try
            {
                var result = await _cobranzaService.InsertarFacturaFormaPago(listado);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseControl() { message = ex.Message.ToString(), Code = "1" });
            }
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ValidarPlanillaFactura([FromBody] ListadoConciliacionDto listado)
        {
            var result = await _cobranzaService.Validar_Planilla_FacturaAsync(listado);
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ObtenerFormaPago([FromBody] Cargarlote cargarJson)
        {
            var result = await _cobranzaService.ObtenerFormaPago(cargarJson.IdBanco, cargarJson.IdProceso);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ProcessConciliaciones([FromForm] FilesUpload Data)
        {
            var result = await _cobranzaService.ProcessConciliaciones(Data.DataFile, Int32.Parse(Data.JsonMaster));
            return Ok(result);
        }

        /* para visa*/
        //[HttpPost("[action]")]
        //public async Task<IActionResult> ProcessConciliaciones([FromForm] FilesUpload Data)
        //{
        //    var result = await _cobranzaService.ProcessConciliacionesVisa(Data.DataFile, Int32.Parse(Data.JsonMaster));
        //    return Ok(result);
        //}


        [HttpPost("[action]")]
        public async Task<IActionResult> ProcessDelCargaDeposito([FromForm] FilesUpload Data)
        {
            var result = await _cobranzaService.ProcessDelCargaDeposito(Data.DataFile, Data.idArchivo, Data.idTransac);
            return Ok(result);
        }
        // SEGUI,MIENTO DE LOTE------- /HOMOGDO 09/11/2021
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarSeguimientoLote([FromBody] DatosConsultaSeguimientoLoteDto datosConsultaSeguimientoLoteDto)
        {
            _logger.LogInfo("Metodo Listar SeguimientoLoteCobranza");

            try
            {
                DateTime dfechaDesde = Convert.ToDateTime(datosConsultaSeguimientoLoteDto.FechaDesde);
                DateTime dfechaHasta = Convert.ToDateTime(datosConsultaSeguimientoLoteDto.FechaHasta);
                datosConsultaSeguimientoLoteDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
                datosConsultaSeguimientoLoteDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");
                var result = await _cobranzaService.ReporteSeguimientoLote(datosConsultaSeguimientoLoteDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Metodo Listar  SeguimientoLote " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }


        //Continuando desde aqui los LISTAR//
        //otro
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarContratanteNC([FromBody] DatosConsultaContratanteNCDto datosConsultaContratanteNCDto)
        {
            _logger.LogInfo("Metodo Listar contratante NC");

            var result = await _cobranzaService.ListarContratanteNC(datosConsultaContratanteNCDto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarReciboPendienteNC([FromBody] DatosConsultaReciboPendienteNCDto datosConsultaReciboPendienteNCDto)
        {
            _logger.LogInfo("Metodo Listar ReciboPay");

            try
            {
                var result = await _cobranzaService.ListarReciboPendienteNC(datosConsultaReciboPendienteNCDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Método Listar  ListarReciboPendiente de Nota de Crédito " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }
        //public async Task<IActionResult> ListarFormaPagoNC([FromBody] DatosConsultaFormaPagoNCDto datosConsultaFormaPagoNCDto)
        //{
        //    _logger.LogInfo("Metodo Listar Forma Pago NC");

        //    try
        //    {
        //        var result = await _cobranzaService.ListarFormaPagoNCDto(datosConsultaFormaPagoNCDto);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInfo("Método Listar  FormaNCDto de Nota de Crédito " + ex.Message);
        //        return Json(new { status = "error", message = ex.Message });
        //    }

        //}
        [HttpPost("[action]")]
        public async Task<IActionResult>ReporteFormaPago([FromBody] DatosConsultaFormaPagoDto datosConsultaFormaPagoDto)
        {
            _logger.LogInfo("Metodo AddPaymentMethod ");

            try
            {
                var result = await _cobranzaService.ReporteFormaPago(datosConsultaFormaPagoDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Método Reporte Forma de Pago " + ex.Message);
                return Json(new { status = "error", message = ex.Message });
            }

        }
        //CONTINUAR POR AQUI --- 29/09/21 15:08HRS


        [HttpPost("[action]")]
        public async Task<IActionResult> ListarTipoFP()
        {
            _logger.LogInfo("Metodo Listar Tipo de Forma de Pago");

            var listaTipoFPResult = await _cobranzaService.ListarTipoFP();
            return Ok(listaTipoFPResult);
        }

        //06/10/2021

        //[HttpPost("[action]")]
        //public async Task<IActionResult> ProcessGenerarPlanillaNC([FromBody] DatosGenerarPlanillaNCDto datosGenerarPlanillaNCDto)
        //{
        //    _logger.LogInfo("Metodo Listar Generar Planilla de NC");

        //    var result = await _cobranzaService.ProcessGenerarPlanillaNC(datosGenerarPlanillaNCDto);
        //    return Ok(result);
        //}


    }

    public class FilesDelete
    {
        public int idTransac { get; set; }
        public int idArchivo { get; set; }
    }

    public class FilesUpload
    {
        public IFormFile DataFile { get; set; }
        public int idArchivo { get; set; }
        public int idTransac { get; set; }
        public string JsonMaster { get; set; }
    }

    public class Cargarlote
    {
        public int IdBanco { get; set; }
        public int IdProducto { get; set; }
        public string IdProceso { get; set; }
        public string UserCode { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string CodProforma { get; set; }
        public string Data { get; set; }
    }
}