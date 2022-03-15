using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.Canal;

namespace Protecta.Application.Service.Services.CanalModule
{
    public interface ICanalService
    {
        Task<IEnumerable<CanalDto>> ListarCanal();
    }
}
