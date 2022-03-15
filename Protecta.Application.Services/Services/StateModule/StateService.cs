using AutoMapper;
using Protecta.Application.Service.Dtos.State;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.StateModule.Aggregates.StateAgg;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.StateModule
{
    public class StateService : IStateService
    {
        private readonly IStateRepository stateRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public StateService(IStateRepository stateRepository,
                            ILoggerManager logger,
                            IMapper mapper)
        {
            this.stateRepository = stateRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StateDto>> GetState()
        {
            IEnumerable<State> entitiesState = await stateRepository.GetState();
            IEnumerable<StateDto> stateDtos = _mapper.Map<IEnumerable<StateDto>>(entitiesState);
            return stateDtos;
        }
    }
}
