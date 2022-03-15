using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Planilla;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg;
using Protecta.CrossCuting.Utilities.Configuration;

namespace Protecta.Application.Service.Services.PlanillaModule
{
    public class PlanillaService : IPlanillaService
    {
        private readonly IPlanillaRepository _planillaRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;        

        public PlanillaService(IPlanillaRepository planillaRepository, ILoggerManager logger, IMapper mapper)
        {
            this._planillaRepository = planillaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<string> ImportarPlanilla(DatosConsultaPlanillaDto datosConsultaPlanillaDto)
        {
            var result = string.Empty;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaPlanilla>(datosConsultaPlanillaDto);               

                var planillaResult = await _planillaRepository.ListarPlanilla(datosConsultaEntity);

                if (planillaResult.Count > 0)
                {
                    await _planillaRepository.RegistrarPlanilla(planillaResult);

                    await _planillaRepository.ActualizarEstadoImportacion(planillaResult); //En INSUDB                  

                    result = "1";
                }
                else
                {
                    result = "0";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return result;
        }

        public async Task<string> NotificarPlanillaConciliada(DatosNotificacionDto datosNotificacionDto)
        {
            DatosNotificacionDto datosNotificacion_aux = null;
            List<DetallePlanillaCobroDto> planillaCertificadoList = new List<DetallePlanillaCobroDto>();
            List<DetallePlanillaCobroDto> planillaCertificadoList_aux = new List<DetallePlanillaCobroDto>();
            PlanillaEstadoDto _planillaEstado = null;

            var _response = string.Empty;

            try
            {
                var datosNotificacionEntity = _mapper.Map<DatosNotificacion>(datosNotificacionDto);

                if (datosNotificacionEntity.IndPlanilla == 0)
                {
                    datosNotificacion_aux = new DatosNotificacionDto();
                    datosNotificacion_aux.Planilla = string.Empty;
                    datosNotificacion_aux.IndPlanilla = 0;
                    datosNotificacion_aux.Usuario = datosNotificacionEntity.Usuario.ToString(); //Agregado 16/07/2018

                    var _entity = _mapper.Map<DatosNotificacion>(datosNotificacion_aux);

                    var _result = await _planillaRepository.ListarPlanillasConciliadas(_entity);

                    planillaCertificadoList_aux = _mapper.Map<List<DetallePlanillaCobroDto>>(_result);

                    foreach (DetallePlanillaCobroDto d in planillaCertificadoList_aux)
                    {
                        planillaCertificadoList.Add(d);
                    }
                }
                else if (datosNotificacionEntity.IndPlanilla == 1)
                {
                    string[] planillas = datosNotificacionEntity.Planilla.Split(",");

                    for (int i = 0; i <= planillas.Length - 1; i++)
                    {
                        datosNotificacion_aux = new DatosNotificacionDto();
                        datosNotificacion_aux.Planilla = planillas[i].ToString();
                        datosNotificacion_aux.IndPlanilla = 1;
                        datosNotificacion_aux.Usuario = datosNotificacionEntity.Usuario.ToString(); //Agregado 16/07/2018

                        var _entity = _mapper.Map<DatosNotificacion>(datosNotificacion_aux);

                        var _result = await _planillaRepository.ListarPlanillasConciliadas(_entity);

                        planillaCertificadoList_aux = _mapper.Map<List<DetallePlanillaCobroDto>>(_result);

                        foreach (DetallePlanillaCobroDto d in planillaCertificadoList_aux)
                        {
                            planillaCertificadoList.Add(d);
                        }
                    }
                }

                foreach (DetallePlanillaCobroDto p in planillaCertificadoList)
                {
                    var _planillaCertificadoEntity = _mapper.Map<DetallePlanillaCobro>(p);

                    _planillaCertificadoEntity.IndicaComprobante = 1;
                    _planillaCertificadoEntity.IndicaComision = 1;
                    _planillaCertificadoEntity.IddgEstadoDetPlanilla = "1105"; //Enviado a generar comprobante 
                    _planillaCertificadoEntity.VcUsuariocreacion = p.VcUsuariocreacion.ToString(); //Agregado 16/07/2018

                    var _operacion = await _planillaRepository.RegistrarComprobantePendiente(_planillaCertificadoEntity);

                    if (_operacion == "1")
                    {
                        //Registra estado de envio a generar comprobante
                        _planillaEstado = new PlanillaEstadoDto();
                        _planillaEstado.IdPlanilla = p.IdPlanilla;
                        _planillaEstado.IddgEstadoplanilla = 1105; //Enviado a generar comprobante
                        _planillaEstado.IddgEstado = 1001; //Registro activo
                        _planillaEstado.VcUsuariocreacion = p.VcUsuariocreacion;

                        var _estado = _mapper.Map<PlanillaEstado>(_planillaEstado);

                        await _planillaRepository.RegistrarEstadoPlanilla(_estado);

                        _response = "1";
                    }
                    else
                    {
                        _response = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return _response;
        }

        public async Task<string> NotificarPlanillaNoLiquidada(DatosNotificacionDto datosNotificacionDto)
        {
            DatosNotificacionDto datosNotificacion_aux = new DatosNotificacionDto();
            List<DetallePlanillaCobroDto> planillaCertificadoList = new List<DetallePlanillaCobroDto>();
            List<DetallePlanillaCobroDto> planillaCertificadoList_aux = new List<DetallePlanillaCobroDto>();
            PlanillaEstadoDto _planillaEstado = null;

            var _response = string.Empty;

            try
            {
                var datosNotificacionEntity = _mapper.Map<DatosNotificacion>(datosNotificacionDto);

                if (datosNotificacionEntity.IndPlanilla == 0)
                {
                    datosNotificacion_aux = new DatosNotificacionDto();
                    datosNotificacion_aux.Planilla = string.Empty;
                    datosNotificacion_aux.IndPlanilla = 0;
                    datosNotificacion_aux.Usuario = datosNotificacionEntity.Usuario.ToString(); //Agregado 16/07/2018

                    var _entity = _mapper.Map<DatosNotificacion>(datosNotificacion_aux);

                    var _result = await _planillaRepository.ListarPlanillasNoConciliadas(_entity);

                    planillaCertificadoList_aux = _mapper.Map<List<DetallePlanillaCobroDto>>(_result);

                    foreach (DetallePlanillaCobroDto d in planillaCertificadoList_aux)
                    {
                        planillaCertificadoList.Add(d);
                    }
                }
                else if (datosNotificacionEntity.IndPlanilla == 1)
                {
                    string[] planillas = datosNotificacionEntity.Planilla.Split(",");

                    for (int i = 0; i <= planillas.Length - 1; i++)
                    {
                        datosNotificacion_aux = new DatosNotificacionDto();
                        datosNotificacion_aux.Planilla = planillas[i].ToString();
                        datosNotificacion_aux.IndPlanilla = 1;
                        datosNotificacion_aux.Usuario = datosNotificacionEntity.Usuario.ToString(); //Agregado 16/07/2018

                        var _entity = _mapper.Map<DatosNotificacion>(datosNotificacion_aux);

                        var _result = await _planillaRepository.ListarPlanillasNoConciliadas(_entity);

                        planillaCertificadoList_aux = _mapper.Map<List<DetallePlanillaCobroDto>>(_result);

                        foreach (DetallePlanillaCobroDto d in planillaCertificadoList_aux)
                        {
                            planillaCertificadoList.Add(d);
                        }
                    }
                }

                var _validaFechaEnvioComprobante = await _planillaRepository.ValidaFechaEnvioComprobante();

                foreach (DetallePlanillaCobroDto p in planillaCertificadoList)
                {
                    var _planillaCertificadoEntity = _mapper.Map<DetallePlanillaCobro>(p);

                    var _validaExisteContratante = await _planillaRepository.ValidarExisteContratante(_planillaCertificadoEntity);                    

                    //Regla #2 && Regla #3 
                    if (_validaFechaEnvioComprobante == "1" || _validaExisteContratante == "1")
                    {
                        _planillaCertificadoEntity.IndicaComprobante = 1;
                        _planillaCertificadoEntity.IndicaComision = 0;
                        _planillaCertificadoEntity.IddgEstadoDetPlanilla = "1105"; //Enviado a generar comprobante 
                        _planillaCertificadoEntity.VcUsuariocreacion = p.VcUsuariocreacion.ToString(); //Agregado 16/07/2018

                        var _operacion = await _planillaRepository.RegistrarComprobantePendiente(_planillaCertificadoEntity);

                        if (_operacion == "1")
                        {
                            //Registra estado de envio a generar comprobante
                            _planillaEstado = new PlanillaEstadoDto();
                            _planillaEstado.IdPlanilla = p.IdPlanilla;
                            _planillaEstado.IddgEstadoplanilla = 1105; //Enviado a generar comprobante
                            _planillaEstado.IddgEstado = 1001; //Registro activo
                            _planillaEstado.VcUsuariocreacion = p.VcUsuariocreacion;

                            var _estado = _mapper.Map<PlanillaEstado>(_planillaEstado);

                            await _planillaRepository.RegistrarEstadoPlanilla(_estado);

                            _response = "1";
                        }
                        else
                        {
                            _response = "2";
                        }
                    }
                    else
                    {
                        _response = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return _response;
        }

        public async Task<IEnumerable<PlanillaConsultaProcesadaDto>> ConsultarPlanillasProcesadas(DatosConsultaPlanillaDto datosConsultaPlanillaDto)
        {
            IEnumerable<PlanillaConsultaProcesadaDto> planillasDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaPlanilla>(datosConsultaPlanillaDto);

                var planillasResult = await _planillaRepository.ConsultarPlanillasProcesadas(datosConsultaEntity);

                if (planillasResult == null)
                    return null;

                planillasDtos = _mapper.Map<IEnumerable<PlanillaConsultaProcesadaDto>>(planillasResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return planillasDtos;
        }

        public async Task<IEnumerable<PlanillaConsultaProcesadaLogDto>> ConsultarPlanillasProcesadasLog(DatosConsultaPlanillaDto datosConsultaPlanillaDto)
        {
            IEnumerable<PlanillaConsultaProcesadaLogDto> planillasLogDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaPlanilla>(datosConsultaPlanillaDto);

                var planillasLogResult = await _planillaRepository.ConsultarPlanillasProcesadasLog(datosConsultaEntity);

                if (planillasLogResult == null)
                    return null;

                planillasLogDtos = _mapper.Map<IEnumerable<PlanillaConsultaProcesadaLogDto>>(planillasLogResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return planillasLogDtos;
        }


        public async Task<IEnumerable<PlanillaDto>> ConsultarPlanillasPendientes(DatosConsultaPlanillaDto datosConsultaPlanillaDto)
        {
            IEnumerable<PlanillaDto> PlanillasDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaPlanilla>(datosConsultaPlanillaDto);

                var planillasResult = await _planillaRepository.ConsultarPlanillasPendientes(datosConsultaEntity);

                if (planillasResult == null)
                    return null;

                PlanillasDtos = _mapper.Map<IEnumerable<PlanillaDto>>(planillasResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return PlanillasDtos;
        }

        public async Task<string> EliminarFacturaDePlanilla(long idPlanilla) {

            var _response = string.Empty;

            try
            {                                  
                _response = await _planillaRepository.EliminarFacturaDePlanilla(idPlanilla);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return _response;
        }

        public async Task<int> ValidarFacturaDePlanilla(long idPlanilla)
        {
            var _response = 0;

            try
            {
                _response = await _planillaRepository.ValidarFacturaDePlanilla(idPlanilla);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return _response;
        }
    }
}
