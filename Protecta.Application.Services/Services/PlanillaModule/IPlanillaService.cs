using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.Planilla;
using Protecta.Application.Service.Dtos.Consulta;

namespace Protecta.Application.Service.Services.PlanillaModule
{
    public interface IPlanillaService 
    {
        Task<string> ImportarPlanilla(DatosConsultaPlanillaDto datosConsultaPlanillaDto);

        Task<string> NotificarPlanillaConciliada(DatosNotificacionDto datosNotificacionDto);

        Task<string> NotificarPlanillaNoLiquidada(DatosNotificacionDto datosNotificacionDto);

        Task<IEnumerable<PlanillaConsultaProcesadaDto>> ConsultarPlanillasProcesadas(DatosConsultaPlanillaDto datosConsultaPlanillaDto);

        Task<IEnumerable<PlanillaConsultaProcesadaLogDto>> ConsultarPlanillasProcesadasLog(DatosConsultaPlanillaDto datosConsultaPlanillaDto);

        Task<IEnumerable<PlanillaDto>> ConsultarPlanillasPendientes(DatosConsultaPlanillaDto datosConsultaPlanillaDto);

        Task<string> EliminarFacturaDePlanilla(long idPlanilla);
        Task<int> ValidarFacturaDePlanilla(long idPlanilla);
    }
}
