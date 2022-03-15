using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Protecta.Application.Service.Dtos.Entidad;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.EntidadModule.Aggregates.EntidadAgg;

namespace Protecta.Application.Service.Services.EntidadModule
{
    public class EntidadService : IEntidadService
    {
        private readonly IEntidadRepository _entidadRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public EntidadService(IEntidadRepository entidadRepository, ILoggerManager logger, IMapper mapper)
        {
            this._entidadRepository = entidadRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CuentaDto>> ListarCuentaxEntidad(long idEntidad)
        {
            IEnumerable<CuentaDto> CuentasDtos = null;

            try
            {
                var cuentaResult = await _entidadRepository.ListarCuentaxEntidad(idEntidad);

                if (cuentaResult == null)
                    return null;

                cuentaResult.Add(new Cuenta { ID_CUENTA = 0, NUMERO_CUENTA = "TODOS" });
                cuentaResult.Reverse();

                CuentasDtos = _mapper.Map<IEnumerable<CuentaDto>>(cuentaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }

            return CuentasDtos;


            ////Traer datos de los productos de base de datos
            //var cuentaResult = await _entidadRepository.ListarCuentaxEntidad(idEntidad);

            ////Verifica que el resultado no sea nulo
            //if (cuentaResult == null)
            //    return null;

            ////Mapeo
            //var CuentasDtos = _mapper.Map<IEnumerable<CuentaDto>>(cuentaResult);

            //return CuentasDtos;
        }

        public async Task<IEnumerable<EntidadDto>> ListarEntidades(bool bTodos)
        {
            IEnumerable<EntidadDto> EntidadesDtos = null;

            try
            {
                var entidadResult = await _entidadRepository.ListarEntidades();

                if (entidadResult == null)
                    return null;

                if (bTodos)
                {
                    entidadResult.Add(new Entidad { ID_ENTIDAD = 0, VC_NOMBRE = "TODOS" });
                    entidadResult.Reverse();
                }


                EntidadesDtos = _mapper.Map<IEnumerable<EntidadDto>>(entidadResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }

            return EntidadesDtos;

            ////Traer datos de los productos de base de datos
            //var entidadResult = await _entidadRepository.ListarEntidades();

            ////Verifica que el resultado no sea nulo
            //if (entidadResult == null)
            //    return null;

            ////Mapeo
            //var EntidadesDtos = _mapper.Map<IEnumerable<EntidadDto>>(entidadResult);

            //return EntidadesDtos;
        }
    }
}
