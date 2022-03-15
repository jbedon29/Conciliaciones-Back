using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.Application.Service.Dtos.TipoArchivo;
using Protecta.Application.Service.Dtos.Conciliacion;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.General;

namespace Protecta.Application.Service.Services.ConciliacionModule
{
    public interface IConciliacionService
    {
        Task<RespuestaDto> AplicarConciliacionAutomatica(DatosAplicaConciliacionDto datosAplicacionDto);

        Task<RespuestaDto> AplicarConciliacionManual(DatosAplicacionManualDto datosAplicacionDto);

        Task<RespuestaDto> RevertirConciliacion(long idPlanilla);
    }
}
