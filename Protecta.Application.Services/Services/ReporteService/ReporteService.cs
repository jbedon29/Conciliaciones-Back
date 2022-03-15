using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.Reporte;
using Protecta.Application.Service.Services.Email;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;

namespace Protecta.Application.Service.Services.ReporteService
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public ReporteService(IReporteRepository reporteRepository, ILoggerManager logger, IMapper mapper)
        {
            this._reporteRepository = reporteRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReporteDepositoPendienteDto>> ReporteDepositosPendientes(DatosReporteConciliacionPendienteDto datosConciliacionPendienteDto)
        {
            IEnumerable<ReporteDepositoPendienteDto> reporteDepositosPendientesDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosReporteConciliacionPendiente>(datosConciliacionPendienteDto);

                var depositosResult = await _reporteRepository.ReporteDepositosPendientes(datosConsultaEntity);

                if (depositosResult == null || depositosResult.Count == 0)
                {
                    depositosResult = new List<ReporteDepositoPendiente>{
                        new ReporteDepositoPendiente()
                    };
                }

                reporteDepositosPendientesDtos = _mapper.Map<IEnumerable<ReporteDepositoPendienteDto>>(depositosResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return reporteDepositosPendientesDtos;
        }

        public async Task<IEnumerable<ReportePlanillaPendienteDto>> ReportePlanillasPendientes(DatosReporteConciliacionPendienteDto datosConciliacionPendienteDto)
        {
            IEnumerable<ReportePlanillaPendienteDto> reportPlanillasPendientesDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosReporteConciliacionPendiente>(datosConciliacionPendienteDto);

                var depositosResult = await _reporteRepository.ReportePlanillasPendientes(datosConsultaEntity);

                if (depositosResult == null || depositosResult.Count == 0)
                {
                    depositosResult = new List<ReportePlanillaPendiente>
                    {
                        new ReportePlanillaPendiente()
                    };
                }

                reportPlanillasPendientesDtos = _mapper.Map<IEnumerable<ReportePlanillaPendienteDto>>(depositosResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return reportPlanillasPendientesDtos;
        }

        public async Task<IEnumerable<ReporteMacroProcesadoDto>> ConsultarReporteMacro(DatosConsultaReporteDto datosConsultaReporteMacroDto)
        {
            IEnumerable<ReporteMacroProcesadoDto> reportesMacroDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReporte>(datosConsultaReporteMacroDto);

                var reportesMacroResult = await _reporteRepository.ConsultarReporteMacro(datosConsultaEntity);

                if (reportesMacroResult == null)
                    return null;

                reportesMacroDtos = _mapper.Map<IEnumerable<ReporteMacroProcesadoDto>>(reportesMacroResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reportesMacroDtos;
        }

        public async Task<IEnumerable<ReportePendientePorImportarProcesadoDto>> ReportePendientesPorImportar()
        {
            IEnumerable<ReportePendientePorImportarProcesadoDto> ReporteDtos = null;

            try
            {
                var ReportResult = await _reporteRepository.ReportePendientesPorImportar();
                if (ReportResult == null) return null;
                ReporteDtos = _mapper.Map<IEnumerable<ReportePendientePorImportarProcesadoDto>>(ReportResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return ReporteDtos;
        }


        public async Task<IEnumerable<ReporteComprobantesPorPlanillaProcesadoDto>> ReporteComprobantesPorPlanilla(DatosConsultaComprobantePorPlanillaDto datosConsultaReporteComprobantePorPlanillaDto)
        {
            IEnumerable<ReporteComprobantesPorPlanillaProcesadoDto> reportesComprobantesPorPlanillaDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReporteComprobante>(datosConsultaReporteComprobantePorPlanillaDto);

                var reportesComprobantesPlanillaResult = await _reporteRepository.ReporteComprobantesPorPlanilla(datosConsultaEntity);

                if (reportesComprobantesPlanillaResult == null)
                    return null;

                reportesComprobantesPorPlanillaDtos = _mapper.Map<IEnumerable<ReporteComprobantesPorPlanillaProcesadoDto>>(reportesComprobantesPlanillaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reportesComprobantesPorPlanillaDtos;
        }
        public async Task<IEnumerable<ReporteListadoErrorProcesadoDto>> ReporteListadoConError(DatosConsultaListadoErrorDto datosConsultaListadoErrorDto)
        {
            IEnumerable<ReporteListadoErrorProcesadoDto> reportesListadoDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReporteListado>(datosConsultaListadoErrorDto);

                var reportesListadoErrorResult = await _reporteRepository.ReporteListadoConError(datosConsultaEntity);

                if (reportesListadoErrorResult == null)
                    return null;

                reportesListadoDtos = _mapper.Map<IEnumerable<ReporteListadoErrorProcesadoDto>>(reportesListadoErrorResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reportesListadoDtos;
        }
        public async Task<Mensaje> ActualizarPlanilla(DatosPlanillaDto datosConsultaListado)
        {
            Mensaje resultActualizar = null;

            try
            {
                var datosPlantilla = _mapper.Map<DatosPlanilla>(datosConsultaListado);

                var result = await _reporteRepository.ActualizarPlanilla(datosPlantilla);

                if (result == null)
                    return null;

                resultActualizar = _mapper.Map<Mensaje>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return resultActualizar;
        }

        public async Task<Mensaje> ComunicarPlanilla(DatosPlanillaRechazarDto datosConsultaListado)
        {
            Mensaje resultActualizar = null;
            EmailService _serviceEmail = new EmailService();
            try
            {
                var datosPlantilla = _mapper.Map<DatosPlanillaRechazar>(datosConsultaListado);
                string result2;
                var result = await _reporteRepository.ComunicarPlanilla(datosPlantilla);
                if (result.ncode == "0") { 
                    result.observacion = datosConsultaListado.observacion;
                    result2 = await _serviceEmail.SenderEmailPlanilla(datosPlantilla.Planilla.ToString() , result, result.nemail);
                }
                if (result == null)
                    return null;

                resultActualizar = _mapper.Map<Mensaje>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return resultActualizar;
        }
        public async Task<IEnumerable<ReporteListadoErrorProcesadoDto>> OperacionDetalle(DatoDetallesPlanillaDto datoDetallesPlanillaDto)
        {
            IEnumerable<ReporteListadoErrorProcesadoDto> reportesDtos = null;

            try
            {
                 var datoDetallesPlanilla = _mapper.Map<DatosDetallePlanilla>(datoDetallesPlanillaDto);

                var OperacionDetalleResult = await _reporteRepository.OperacionDetalle(datoDetallesPlanilla);

                if (OperacionDetalleResult == null)
                    return null;

                reportesDtos = _mapper.Map<IEnumerable<ReporteListadoErrorProcesadoDto>>(OperacionDetalleResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reportesDtos;
        }
        public async Task<IEnumerable<ReporteExtractoBancarioDto>> ReporteExtractoBancario(DatosConsultaExtractoDto datosConsultaExtractoDto)
        {
            IEnumerable<ReporteExtractoBancarioDto> reportesExtractoDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaExtracto>(datosConsultaExtractoDto);

                var reportesExtractoBancariorResult = await _reporteRepository.ReporteExtractoBancario(datosConsultaEntity);

                if (reportesExtractoBancariorResult == null)
                    return null;

                reportesExtractoDtos = _mapper.Map<IEnumerable<ReporteExtractoBancarioDto>>(reportesExtractoBancariorResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reportesExtractoDtos;
        }
    }
}
