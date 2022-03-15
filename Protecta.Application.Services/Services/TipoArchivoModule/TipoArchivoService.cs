using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.TipoArchivo;
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;

namespace Protecta.Application.Service.Services.TipoArchivoModule
{
    public class TipoArchivoService : ITipoArchivoService
    {
        private readonly ITipoArchivoRepository _tipoarchivoRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public TipoArchivoService(ITipoArchivoRepository tipoarchivoRepository, ILoggerManager logger, IMapper mapper)
        {
            this._tipoarchivoRepository = tipoarchivoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TipoArchivoDto>> ListarTipoArchivo()
        {
            IEnumerable<TipoArchivoDto> tipoArchivoDtos = null;

            try
            {
                var tipoarchivoResult = await _tipoarchivoRepository.ListarTipoArchivo();

                if (tipoarchivoResult == null) return null;

                tipoArchivoDtos = _mapper.Map<IEnumerable<TipoArchivoDto>>(tipoarchivoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return tipoArchivoDtos;
        }
    }
}
