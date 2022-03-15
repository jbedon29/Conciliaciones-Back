using Protecta.Application.Service.Dtos.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.StateModule
{
    public interface IStateService
    {
        Task<IEnumerable<StateDto>> GetState();
    }
}
