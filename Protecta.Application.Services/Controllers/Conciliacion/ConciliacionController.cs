using System;
using AutoMapper;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Conciliacion;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Services.ConciliacionModule;

using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Application.Service.Services.PlanillaModule;

namespace Protecta.Application.Service.Controllers.Conciliacion
{
    [Route("api/[controller]")]
    public class ConciliacionController : Controller
    {
        private IConciliacionService _conciliacionService;
        private IPlanillaService _planillaService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public ConciliacionController(ILoggerManager logger, 
            IConciliacionService _conciliacionService,
            IPlanillaService _planillaService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            this._conciliacionService = _conciliacionService;
            this._planillaService = _planillaService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]        
        public async Task<IActionResult> AplicarConciliacionAutomatica([FromBody] DatosAplicaConciliacionDto datosAplicacionDto)            
        {
            _logger.LogInfo("Metodo aplicar conciliacion automatica");

            if (datosAplicacionDto.IdProducto!= 71)
            {
                datosAplicacionDto.IdTipoCobro = 0;
            }
            var conciliacionResult = await _conciliacionService.AplicarConciliacionAutomatica(datosAplicacionDto);

            if (conciliacionResult.Resultado == 1)
            {
                conciliacionResult.Mensaje = "La conciliación automática se realizó correctamente.";
            }

            try
            {
                DatosNotificacionDto datosNotificacionDto = new DatosNotificacionDto();
                datosNotificacionDto.IndPlanilla = 0;
                datosNotificacionDto.Planilla = "0";
                datosNotificacionDto.Usuario = datosAplicacionDto.Usuario.ToString(); //Agregado 16/07/2018

                var res =  await _planillaService.NotificarPlanillaConciliada(datosNotificacionDto);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }

            return Ok(conciliacionResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AplicarConciliacionManual([FromBody] DatosAplicacionManualDto datosAplicacionDto)
        {
            _logger.LogInfo("Metodo aplicar conciliacion manual");            

            var conciliacionResult = await _conciliacionService.AplicarConciliacionManual(datosAplicacionDto);

            if (conciliacionResult.Resultado == 1)
            {
                conciliacionResult.Mensaje = "El proceso termino con exito.";
            }

            //Agregado 16/07/2018
            try
            {
                DatosNotificacionDto datosNotificacionDto = new DatosNotificacionDto();
                datosNotificacionDto.IndPlanilla = 0;
                datosNotificacionDto.Planilla = "0";
                datosNotificacionDto.Usuario = datosAplicacionDto.Usuario.ToString(); //Agregado 16/07/2018

                var res = await _planillaService.NotificarPlanillaConciliada(datosNotificacionDto);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }

            return Ok(conciliacionResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RevertirConciliacion([FromBody]long idPlanilla)
        {
            _logger.LogInfo("Metodo aplicar RevertirConciliacion");

            var conciliacionResult = await _conciliacionService.RevertirConciliacion(idPlanilla);          

            if (conciliacionResult.Resultado == 1)
            {
                var deletebillResult = await _planillaService.EliminarFacturaDePlanilla(idPlanilla);
                conciliacionResult.Mensaje =string.IsNullOrEmpty(deletebillResult)? "El proceso termino con exito.": deletebillResult;
            }          

            return Ok(conciliacionResult);
        }
    }
}
