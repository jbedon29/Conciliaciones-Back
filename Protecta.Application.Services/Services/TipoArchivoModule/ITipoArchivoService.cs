using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.TipoArchivo;

namespace Protecta.Application.Service.Services.TipoArchivoModule
{
    public interface ITipoArchivoService
    {
        Task<IEnumerable<TipoArchivoDto>> ListarTipoArchivo();
    }
}