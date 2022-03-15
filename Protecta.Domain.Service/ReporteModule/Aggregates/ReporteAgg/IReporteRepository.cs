using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;

namespace Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg
{
    public interface IReporteRepository
    {
        Task<List<ReporteDepositoPendiente>> ReporteDepositosPendientes(DatosReporteConciliacionPendiente datosConsulta);
        Task<List<ReportePlanillaPendiente>> ReportePlanillasPendientes(DatosReporteConciliacionPendiente datosConsulta);
        Task<List<ReporteConsultaProcesada>> ConsultarReporteMacro(DatosConsultaReporte datosConsulta);       
        Task<List<ReportePendienteImportarProcesada>> ReportePendientesPorImportar();
        Task<List<ReporteComprobantePlanillaProcesada>> ReporteComprobantesPorPlanilla(DatosConsultaReporteComprobante datosConsulta);

        // AGREGADO PA
        Task<List<ReporteListadoError>> ReporteListadoConError(DatosConsultaReporteListado datosConsulta);

        Task<Mensaje> ActualizarPlanilla(DatosPlanilla datosPlanilla); 
        Task<Mensaje> ComunicarPlanilla(DatosPlanillaRechazar datosPlanillaRechazar);
        Task<List<ReporteListadoError>> OperacionDetalle(DatosDetallePlanilla datosDetallePlanilla);
        Task<List<ReporteExtracto>> ReporteExtractoBancario(DatosConsultaExtracto datosConsulta);
    }
}
