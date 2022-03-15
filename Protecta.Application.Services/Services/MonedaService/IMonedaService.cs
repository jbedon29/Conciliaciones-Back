using Protecta.Application.Service.Dtos.Moneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.MonedaService
{
    public interface IMonedaService
    {
        Task<IEnumerable<MonedaDto>> ListarMoneda();
    }
}
