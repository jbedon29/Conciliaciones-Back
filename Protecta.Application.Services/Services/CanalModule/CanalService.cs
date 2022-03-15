using System;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Canal;
using Protecta.Domain.Service.CanalModule.Aggregates.CanalAgg;

namespace Protecta.Application.Service.Services.CanalModule
{
    public class CanalService : ICanalService
    {
        private readonly ICanalRepository _canalRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public CanalService(ICanalRepository canalRepository, ILoggerManager logger, IMapper mapper)
        {
            this._canalRepository = canalRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CanalDto>> ListarCanal()
        {
            IEnumerable<CanalDto> CanalDtos = null;

            try
            {
                var canalResult = await _canalRepository.ListarCanal();

                if (canalResult == null) return null;

                canalResult.Add(new Canal { IdCanal = 0, DescripcionCanal = "TODOS" });
                canalResult.Reverse();

                CanalDtos = _mapper.Map<IEnumerable<CanalDto>>(canalResult);                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return CanalDtos;
        }
    }
}
