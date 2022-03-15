using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg;

namespace Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg
{
    public interface IConciliacionRepository
    {        
        Task<Respuesta> AplicarConciliacionAutomatica(DatosAplicaConciliacion datosAplicacion);

        Task<Respuesta> AplicarConciliacionManual(DatosAplicacionManual datosAplicacion);

        Task<Respuesta> RevertirConciliacion(long idPlanilla);
    }
}
