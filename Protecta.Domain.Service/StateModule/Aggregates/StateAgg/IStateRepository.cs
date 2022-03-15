using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Protecta.Domain.Interfaces.Repository.Common;

namespace Protecta.Domain.Service.StateModule.Aggregates.StateAgg
{
    public interface IStateRepository
    {
        Task<IEnumerable<State>> GetState();
    }
}
