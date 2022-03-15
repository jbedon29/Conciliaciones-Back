using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Services.ReporteService;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;

using Protecta.Application.Service.Services.Email;
namespace Protecta.Application.Service.Controllers.Reporte
{
    [Route("api/[controller]")]
    public class ReporteController : Controller
    {
        private IReporteService _reporteService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public ReporteController(ILoggerManager logger, IReporteService _reporteService,  IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            this._reporteService = _reporteService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ReportePlanillasPendientes([FromBody] DatosReporteConciliacionPendienteDto datosReporteDto)
        //public async Task<IActionResult> ReportePlanillasPendientes()
        {
            _logger.LogInfo("Metodo ReportePlanillasPendientes");

            //DatosReporteConciliacionPendienteDto datosReporteDto = new DatosReporteConciliacionPendienteDto();
            //datosReporteDto.IdProducto = 1000;

            var conciliacionResult = await _reporteService.ReportePlanillasPendientes(datosReporteDto);

            return Ok(conciliacionResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ReporteDepositosPendientes([FromBody] DatosReporteConciliacionPendienteDto datosReporteDto)
        //public async Task<IActionResult> ReporteDepositosPendientes()
        {
            _logger.LogInfo("Metodo ReporteDepositosPendientes");
            //DatosReporteConciliacionPendienteDto datosReporteDto = new DatosReporteConciliacionPendienteDto();
            var conciliacionResult = await _reporteService.ReporteDepositosPendientes(datosReporteDto);

            return Ok(conciliacionResult);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ConsultarReporteMacro([FromBody] DatosConsultaReporteDto datosConsultaReporteMacroDto)
        {

            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaReporteMacroDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaReporteMacroDto.FechaHasta);
            datosConsultaReporteMacroDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaReporteMacroDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var reportResult = await _reporteService.ConsultarReporteMacro(datosConsultaReporteMacroDto);
            return Ok(reportResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ReportePendientesPorImportar()
        {
            var reportResult = await _reporteService.ReportePendientesPorImportar();
            return Ok(reportResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ReporteComprobantesPorPlanilla([FromBody] DatosConsultaComprobantePorPlanillaDto datosConsultaReporteComprobantePorPlanillaDto)
        {
            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaReporteComprobantePorPlanillaDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaReporteComprobantePorPlanillaDto.FechaHasta);
            datosConsultaReporteComprobantePorPlanillaDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaReporteComprobantePorPlanillaDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var reportResult = await _reporteService.ReporteComprobantesPorPlanilla(datosConsultaReporteComprobantePorPlanillaDto);
            return Ok(reportResult);
        }

        // add for PA
        [HttpPost("[action]")]
        public async Task<IActionResult> ReporteListadoConError([FromBody] DatosConsultaListadoErrorDto datosConsultaReporteListadoErrorDto)
        {

            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaReporteListadoErrorDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaReporteListadoErrorDto.FechaHasta);
            datosConsultaReporteListadoErrorDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaReporteListadoErrorDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var reportResult = await _reporteService.ReporteListadoConError(datosConsultaReporteListadoErrorDto);
            return Ok(reportResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> OperacionDetalle([FromBody] DatoDetallesPlanillaDto datoDetallesPlanillaDto)
        {
            DateTime dfecha_deposito = Convert.ToDateTime(datoDetallesPlanillaDto.fechaDeposito);
            datoDetallesPlanillaDto.fechaDeposito = dfecha_deposito.ToString("dd/MM/yyyy");
            var result = await _reporteService.OperacionDetalle(datoDetallesPlanillaDto);
            return Ok(result);
       }

        [HttpPost("[action]")]
        public async Task<IActionResult> ActualizarPlanilla([FromBody] DatosPlanillaDto datosPlanillaDto)
        {
            DateTime dfecha_deposito = Convert.ToDateTime(datosPlanillaDto.FechaDeposito);
            datosPlanillaDto.FechaDeposito = dfecha_deposito.ToString("dd/MM/yyyy");
            var result = await _reporteService.ActualizarPlanilla(datosPlanillaDto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ComunicarPlanilla([FromBody] DatosPlanillaRechazarDto datosPlanillaDto)
        {
            var result = await _reporteService.ComunicarPlanilla(datosPlanillaDto);
            return Ok(result);
        }
        //Extracto bancario for PA
        [HttpPost("[action]")]
        public async Task<IActionResult> ReporteExtractoBancario([FromBody] DatosConsultaExtractoDto datosConsultaExtractoDto)
        {

            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaExtractoDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaExtractoDto.FechaHasta);
            datosConsultaExtractoDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaExtractoDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var reportResult = await _reporteService.ReporteExtractoBancario(datosConsultaExtractoDto);
            return Ok(reportResult);
        }
    }
}