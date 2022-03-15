using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg
{
    public interface ITipoArchivoRepository
    {
        Task<List<TipoArchivo>> ListarTipoArchivo();
    }
}
