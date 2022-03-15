using AutoMapper;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.Deposito;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.DepositoModule
{
    public class DepositoService : IDepositoService
    {
        private readonly IDepositoRepository _depositoRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public DepositoService(IDepositoRepository planillaRepository, ILoggerManager logger, IMapper mapper)
        {
            this._depositoRepository = planillaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepositoDto>> ConsultarDepositosExtornado(DatosConsultaDepositoExtDto datosConsultaDepositoDto)
        {
            IEnumerable<DepositoDto> DepositosDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaDepositoExtorno>(datosConsultaDepositoDto);

                var depositosResult = await _depositoRepository.ListarDepositoExtornado(datosConsultaEntity);

                if (depositosResult == null)
                    return null;

                DepositosDtos = _mapper.Map<IEnumerable<DepositoDto>>(depositosResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }

            return DepositosDtos;
        }

        public async Task<IEnumerable<DepositoDto>> ConsultarDepositosPendientes(DatosConsultaDepositoDto datosConsultaDepositoDto)
        {
            IEnumerable<DepositoDto> DepositosDtos = null;
            
            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaDeposito>(datosConsultaDepositoDto);

                var depositosResult = await _depositoRepository.ListarDeposito(datosConsultaEntity);

                if (depositosResult == null)
                    return null;

                DepositosDtos = _mapper.Map<IEnumerable<DepositoDto>>(depositosResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }

            return DepositosDtos;


            //var datosConsultaEntity = _mapper.Map<DatosConsultaDeposito>(datosConsultaDepositoDto);
            ////Traer datos de los productos de base de datos
            //var planillasResult = await _depositoRepository.ListarDeposito(datosConsultaEntity);

            ////Verifica que el resultado no sea nulo
            //if (planillasResult == null)
            //    return null;

            ////Mapeo
            //var DepositosDtos = _mapper.Map<IEnumerable<DepositoDto>>(planillasResult);

            //return DepositosDtos;
        }
    }
}
