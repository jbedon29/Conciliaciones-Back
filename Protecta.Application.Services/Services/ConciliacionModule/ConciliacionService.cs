using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Conciliacion;
using Protecta.Application.Service.Dtos.TipoArchivo;
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;
using Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Application.Service.Dtos.General;
using Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg;
using System;

namespace Protecta.Application.Service.Services.ConciliacionModule
{
    public class ConciliacionService : IConciliacionService
    {
        private readonly IConciliacionRepository _conciliacionRepository;       
        private ILoggerManager _logger;
        private IMapper _mapper;

        public ConciliacionService(
            IConciliacionRepository conciliacionRepository,           
            ILoggerManager logger,
            IMapper mapper)
        {
            this._conciliacionRepository = conciliacionRepository;
            //this._planillaRepository = planillaRepository;
            //this._depositoRepository = depositoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RespuestaDto> AplicarConciliacionAutomatica(DatosAplicaConciliacionDto datosAplicacionDto)
        {
            //RespuestaDto resultDto = null;
            Respuesta result = new Respuesta();
            try
            {
                _logger.LogInfo("INICIO - AplicarConciliacionAutomatica");
                var conciliacionEntity = _mapper.Map<DatosAplicaConciliacion>(datosAplicacionDto);

                result = await _conciliacionRepository.AplicarConciliacionAutomatica(conciliacionEntity);

                // Verifica que el resultado no sea nulo
                if (result == null)
                    return null;

                //Se mapea el resultado de Producto;
                //resultDto = _mapper.Map<RespuestaDto>(result);
                _logger.LogInfo("FIN - AplicarConciliacionAutomatica");
            }
            catch (Exception ex)
            {
                result.Resultado = -1;
                result.Mensaje = "Hubo un error al realizar la conciliación";

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }

            var resultDto = _mapper.Map<RespuestaDto>(result);

            return resultDto;
        }

        public async Task<RespuestaDto> AplicarConciliacionManual(DatosAplicacionManualDto datosAplicacionDto)
        {
            var conciliacionEntity = _mapper.Map<DatosAplicacionManual>(datosAplicacionDto);

            //Validaciones
            Respuesta result = new Respuesta();
            try
            {
                _logger.LogInfo("INICIO - AplicarConciliacionManual");

                if (conciliacionEntity.IdPlanillas.Length <= 1 || conciliacionEntity.IdDepositos.Length <= 1)
                {
                    result.Resultado = -1;
                    result.Mensaje = "Seleccione Planillas y Depósitos";
                    return _mapper.Map<RespuestaDto>(result);
                }

                conciliacionEntity.IdPlanillas = conciliacionEntity.IdPlanillas.Substring(0, conciliacionEntity.IdPlanillas.Length - 1);
                conciliacionEntity.IdDepositos = conciliacionEntity.IdDepositos.Substring(0, conciliacionEntity.IdDepositos.Length - 1);
                conciliacionEntity.Usuario = datosAplicacionDto.Usuario.ToString();


                result = await _conciliacionRepository.AplicarConciliacionManual(conciliacionEntity);
                if (result == null)
                    return null;

                _logger.LogInfo("FIN - AplicarConciliacionManual");
            }
            catch (System.Exception ex)
            {
                result.Resultado = -1;
                result.Mensaje = "Hubo un error al realizar la conciliación";

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }   

            //Se mapea el resultado de Producto;
            var resultDto = _mapper.Map<RespuestaDto>(result);

            return resultDto;
        }

        

        public async Task<RespuestaDto> RevertirConciliacion(long idPlanilla)
        {
            //Validaciones
            Respuesta result = new Respuesta();
            try
            {
                _logger.LogInfo("INICIO - Reversion conciliación");

                if (idPlanilla <= 1 )
                {
                    result.Resultado = -1;
                    result.Mensaje = "Seleccione Planillas y Depósitos";
                    return _mapper.Map<RespuestaDto>(result);
                }
                
                result = await _conciliacionRepository.RevertirConciliacion(idPlanilla);
                if (result == null)
                    return null;

                _logger.LogInfo("FIN - Reversion conciliación");
            }
            catch (System.Exception ex)
            {
                result.Resultado = -1;
                result.Mensaje = "Hubo un error al realizar la conciliación";

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.ToString());
            }

            //Se mapea el resultado de Producto;
            var resultDto = _mapper.Map<RespuestaDto>(result);

            return resultDto;

        }
    }
}
