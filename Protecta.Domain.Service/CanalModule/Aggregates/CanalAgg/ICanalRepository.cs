using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Protecta.Domain.Service.CanalModule.Aggregates.CanalAgg
{
    public interface ICanalRepository
    {
        Task<List<Canal>> ListarCanal();
    }
}
