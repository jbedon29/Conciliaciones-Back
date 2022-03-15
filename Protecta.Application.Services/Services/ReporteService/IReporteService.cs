
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.Reporte;
using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;

namespace Protecta.Application.Service.Services.ReporteService
{
    public interface IReporteService
    {
        Task<IEnumerable<ReportePlanillaPendienteDto>> ReportePlanillasPendientes(DatosReporteConciliacionPendienteDto datosConciliacionPendienteDto);

        Task<IEnumerable<ReporteDepositoPendienteDto>> ReporteDepositosPendientes(DatosReporteConciliacionPendienteDto datosConciliacionPendienteDto);

        Task<IEnumerable<ReporteMacroProcesadoDto>> ConsultarReporteMacro(DatosConsultaReporteDto datosConsultaMacro);       

        Task<IEnumerable<ReportePendientePorImportarProcesadoDto>> ReportePendientesPorImportar();

        Task<IEnumerable<ReporteComprobantesPorPlanillaProcesadoDto>> ReporteComprobantesPorPlanilla(DatosConsultaComprobantePorPlanillaDto datosConsultaMacro);

        // agregado for PA
        Task<IEnumerable<ReporteListadoErrorProcesadoDto>> ReporteListadoConError(DatosConsultaListadoErrorDto datosConsultaListado);

        Task<Mensaje> ActualizarPlanilla(DatosPlanillaDto datosPlanilla);

        Task<Mensaje> ComunicarPlanilla(DatosPlanillaRechazarDto datosPlanilla);
        
        Task<IEnumerable<ReporteListadoErrorProcesadoDto>> OperacionDetalle(DatoDetallesPlanillaDto datoDetallesPlanillaDto);

        Task<IEnumerable<ReporteExtractoBancarioDto>> ReporteExtractoBancario(DatosConsultaExtractoDto datosConsultaExtracto);
    }
}
