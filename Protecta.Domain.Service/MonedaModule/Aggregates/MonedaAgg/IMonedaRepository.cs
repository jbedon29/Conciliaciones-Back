using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.MonedaModule.Aggregates.MonedaAgg
{
    public interface IMonedaRepository
    {
        Task<List<Moneda>> ListarMoneda();
    }
}
