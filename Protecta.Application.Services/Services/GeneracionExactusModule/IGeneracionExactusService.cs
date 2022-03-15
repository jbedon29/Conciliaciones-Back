using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.Consulta;

namespace Protecta.Application.Service.Services.GeneracionExactusModule
{
    public interface IGeneracionExactusService
    {
        Task<string> GenerarInterfaz(DatosConsultaArchivosDto datosConsultaArchivosDto);
    }
}
