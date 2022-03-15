using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.Planilla;
using Protecta.Application.Service.Dtos.General;
using Protecta.Application.Service.Dtos.GeneracionExactus;
using Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;

namespace Protecta.Application.Service.Services.GeneracionExactusModule
{
    public class GeneracionExactusService : IGeneracionExactusService
    {
        private readonly IGeneracionExactusRepository _generacionexactusRepository;
        private readonly IPlanillaRepository _planillaRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public GeneracionExactusService(IGeneracionExactusRepository generacionexactusRepository, IPlanillaRepository _planillaRepository, ILoggerManager logger, IMapper mapper)
        {
            this._generacionexactusRepository = generacionexactusRepository;
            this._planillaRepository = _planillaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<string> GenerarInterfaz(DatosConsultaArchivosDto datosConsultaArchivosDto)
        {
            var result = string.Empty;

            try
            {
                var datosConsultaArcEntity = _mapper.Map<DatosConsultaArchivos>(datosConsultaArchivosDto);

                switch (datosConsultaArcEntity.IdTipoArchivo)
                {
                    case 1:
                        result = await ProcesarDocumentosLiquidados(datosConsultaArcEntity);
                        break;

                    case 2:
                        result = await ProcesarControlBancario(datosConsultaArcEntity);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return result;
        }

        public Task<string> ProcesarDocumentosLiquidados(DatosConsultaArchivos datosConsultaArchivos)
        {
            List<PlanillaEstadoDto> _planillaEstadoList = new List<PlanillaEstadoDto>();

            List<PREMIUM_MO> _reciboAbonoList = null;
            PlanillaDetalle _planillaDetalle = null;
            List<DetallePlanillaCobro> _listaRecibos = null;
            PlanillaEstado _planillaEstado = null;

            string _sMessage = string.Empty;
            
            datosConsultaArchivos.FechaDesde = Convert.ToDateTime(datosConsultaArchivos.FechaDesde).ToShortDateString();
            datosConsultaArchivos.FechaHasta = Convert.ToDateTime(datosConsultaArchivos.FechaHasta).ToShortDateString();            

            try
            {
                //1.Obtiene información de las planillas conciliadas
                var _obj0 = _generacionexactusRepository.ListarPlanillaConciliada(datosConsultaArchivos);

                _planillaEstadoList = _mapper.Map<List<PlanillaEstadoDto>>(_obj0);

                if (_planillaEstadoList.Count > 0)
                {
                    foreach (PlanillaEstadoDto e in _planillaEstadoList)
                    {
                        var _obj1 = _mapper.Map<PlanillaEstado>(e);

                        //Registra Planilla Detalle - Nuevo SP
                        _obj1.VcUsuariocreacion = e.VcUsuariocreacion.ToString();
                        _generacionexactusRepository.RegistrarPlanillaDetalle(_obj1);

                        //Obtiene los recibos
                        _listaRecibos = new List<DetallePlanillaCobro>();
                        _listaRecibos = _generacionexactusRepository.ListarPlanillaCertificado(_obj1);

                        //Obtiene datos del abono
                        foreach (DetallePlanillaCobro c in _listaRecibos)
                        {
                            _planillaDetalle = new PlanillaDetalle();
                            _planillaDetalle.IdProforma = c.IdProforma;

                            _reciboAbonoList = new List<PREMIUM_MO>();
                            _reciboAbonoList = _generacionexactusRepository.ListarReciboAbonado(_planillaDetalle);

                            //Registra en PREMIUM_MO 
                            foreach (PREMIUM_MO p in _reciboAbonoList)
                            {
                                _generacionexactusRepository.RegistrarReciboAbonado(p);
                            }
                        }

                        //Registra estados
                        _planillaEstado = new PlanillaEstado();
                        _planillaEstado.IdPlanilla = e.IdPlanilla;
                        _planillaEstado.IddgEstadoplanilla = 1104; //Documentos Liquidados
                        _planillaEstado.IddgEstado = 1001; //Registro activo
                        _planillaEstado.VcUsuariocreacion = e.VcUsuariocreacion.ToString(); //Agregado 16/07/2018
                        _planillaRepository.RegistrarEstadoPlanilla(_planillaEstado);
                    }

                    _sMessage = "1";
                }
                else
                {
                    _sMessage = "2";
                }              
            }
            catch (Exception ex)
            {
                _sMessage = "0";

                _logger.LogError(ex.InnerException.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

        public Task<string> ProcesarControlBancario(DatosConsultaArchivos datosConsultaArchivos)
        {
            List<PlanillaEstadoDto> _planillaEstadoList = new List<PlanillaEstadoDto>();

            List<COLFORMREF> _cobroAbonoList = null;
            PlanillaDetalle _planillaDetalle = null;
            List<DetallePlanillaCobro> _listaRecibos = null;
            PlanillaEstado _planillaEstado = null;

            string _sMessage = string.Empty;

            datosConsultaArchivos.FechaDesde = Convert.ToDateTime(datosConsultaArchivos.FechaDesde).ToShortDateString();
            datosConsultaArchivos.FechaHasta = Convert.ToDateTime(datosConsultaArchivos.FechaHasta).ToShortDateString();

            try
            {

                //1.Obtiene información de las planillas con documentos liquidados
                var _obj0 = _generacionexactusRepository.ListarPlanillaDocumentoLiquidado(datosConsultaArchivos);

                _planillaEstadoList = _mapper.Map<List<PlanillaEstadoDto>>(_obj0);

                if (_planillaEstadoList.Count > 0)
                {
                    foreach (PlanillaEstadoDto e in _planillaEstadoList)
                    {
                        var _obj1 = _mapper.Map<PlanillaEstado>(e);

                        //Obtiene los recibos
                        _listaRecibos = new List<DetallePlanillaCobro>();
                        _listaRecibos = _generacionexactusRepository.ListarPlanillaCertificado(_obj1);

                        //Obtiene datos del control bancario
                        foreach (DetallePlanillaCobro c in _listaRecibos)
                        {
                            _planillaDetalle = new PlanillaDetalle();
                            _planillaDetalle.IdProforma = c.IdProforma;

                            _cobroAbonoList = new List<COLFORMREF>();
                            _cobroAbonoList = _generacionexactusRepository.ListarCobroAbonado(_planillaDetalle);

                            //Registra en COLFORMREF 
                            foreach (COLFORMREF p in _cobroAbonoList)
                            {
                                _generacionexactusRepository.RegistrarCobroAbonado(p);

                                //Registra comisiones
                                ProcesarComisiones(c.IdProforma);
                            }
                        }

                        //Registra estados
                        _planillaEstado = new PlanillaEstado();
                        _planillaEstado.IdPlanilla = e.IdPlanilla;
                        _planillaEstado.IddgEstadoplanilla = 1107; //Control Bancario
                        _planillaEstado.IddgEstado = 1001; //Registro activo
                        _planillaEstado.VcUsuariocreacion = e.VcUsuariocreacion.ToString(); //Agregado 16/07/2018
                        _planillaRepository.RegistrarEstadoPlanilla(_planillaEstado);
                    }

                    _sMessage = "1";
                }
                else
                {
                    _sMessage = "2";
                }                     
            }
            catch (Exception ex)
            {
                _sMessage = "0";
                _logger.LogError(ex.InnerException.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

        public Task<string> ProcesarComisiones(long idProforma)
        {
            List<COMM_VISANET> _comm_visanetList = null;
            List<DET_COMM_VISANET> _det_comm_visanetList = null;

            string _sMessage = string.Empty;

            try
            {
                _comm_visanetList = new List<COMM_VISANET>();
                _comm_visanetList = _generacionexactusRepository.ListarComisionCabeceraAbonado(idProforma);

                foreach (COMM_VISANET c in _comm_visanetList)
                {
                    _generacionexactusRepository.RegistrarComisionCabeceraAbonado(c);
                }

                _det_comm_visanetList = new List<DET_COMM_VISANET>();
                _det_comm_visanetList = _generacionexactusRepository.ListarComisionDetalleAbonado(idProforma);

                foreach (DET_COMM_VISANET d in _det_comm_visanetList)
                {
                    _generacionexactusRepository.RegistrarComisionDetalleAbonado(d);
                }

                _sMessage = "1";
            }
            catch (Exception ex)
            {
                _sMessage = "0";

                _logger.LogError(ex.InnerException.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

    }
}
