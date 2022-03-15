using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;

namespace Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg
{
    public interface IPlanillaRepository
    {
        Task<List<Planilla>> ListarPlanilla(DatosConsultaPlanilla datosConsultaPlanilla);

        Task<string> RegistrarPlanilla(List<Planilla> planillaList);

        Task<string> ActualizarEstadoImportacion(List<Planilla> planillaList);

        Task<List<DetallePlanillaCobro>> ListarPlanillasConciliadas(DatosNotificacion datosNotificacion);

        Task<List<DetallePlanillaCobro>> ListarPlanillasNoConciliadas(DatosNotificacion datosNotificacion);

        Task<string> ValidarExisteContratante(DetallePlanillaCobro planillaCertificado);

        Task<string> ValidaFechaEnvioComprobante();

        Task<string> RegistrarEstadoPlanilla(PlanillaEstado planillaEstado);

        Task<string> RegistrarComprobantePendiente(DetallePlanillaCobro planillaCertificado);

        Task<List<PlanillaConsultaProcesada>> ConsultarPlanillasProcesadas(DatosConsultaPlanilla datosConsultaPlanilla);

        Task<List<PlanillaConsultaProcesadaLog>> ConsultarPlanillasProcesadasLog(DatosConsultaPlanilla datosConsultaPlanilla);

        Task<List<Planilla>> ConsultarPlanillasPendientes(DatosConsultaPlanilla datosConsultaPlanilla);

        Task<string> EliminarFacturaDePlanilla(long idPlanilla);

        Task<int> ValidarFacturaDePlanilla(long idPlanilla);

    }
}
