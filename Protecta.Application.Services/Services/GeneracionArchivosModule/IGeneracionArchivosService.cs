using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.GeneracionArchivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.GeneracionArchivosModule
{
    public interface IGeneracionArchivosService
    {
        Task<List<DatosRespuestaGeneracionArchivo>> ConsultarArchivos(DatosConsultaGeneracionArchivoDto datosConsultaArchivosDto);
        Task<DatosProcesoGeneracionArchivo> ProcesarArchivos(DatosConsultaGeneracionArchivoDto datosConsultaArchivosDto);
        Task<DatosRespuestaFechaGeneracionDto> FechaGeneracion();
    }
}
