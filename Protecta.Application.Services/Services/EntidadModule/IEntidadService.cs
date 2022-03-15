using Protecta.Application.Service.Dtos.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.EntidadModule
{
    public interface IEntidadService
    {
        Task<IEnumerable<EntidadDto>> ListarEntidades(bool bTodos);
        Task<IEnumerable<CuentaDto>> ListarCuentaxEntidad(long idEntidad);
    }
}
