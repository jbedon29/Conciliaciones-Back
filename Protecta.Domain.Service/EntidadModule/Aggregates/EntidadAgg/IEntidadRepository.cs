using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.EntidadModule.Aggregates.EntidadAgg
{
    public interface IEntidadRepository
    {
        Task<List<Entidad>> ListarEntidades();

        Task<List<Cuenta>> ListarCuentaxEntidad(long idEntidad);


    }
}
