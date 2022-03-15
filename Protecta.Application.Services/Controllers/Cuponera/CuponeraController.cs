using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Cuponera;
using Protecta.Application.Service.Services.CuponeraModule;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg;

namespace Protecta.Application.Service.Controllers.Cuponera
{
   
    [Route("api/Cuponera")]
    public class CuponeraController : Controller
    {
        private ICuponeraService _cuponeraService;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CuponeraController(ILoggerManager logger,
                ICuponeraService _cobranzaService,
                IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            this._cuponeraService = _cobranzaService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ListarTransaciones()
        {
            _logger.LogInfo("Metodo Listar Transaciones");

            var TransacionesResult = await _cuponeraService.ListarTransaciones();
            return Ok(TransacionesResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetInfoRecibo([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo Listar Obtener info Recibo");

            var Result = await _cuponeraService.GetInfoRecibo(parametersReciboDto);
            return Ok(Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetInfoCuponPreview([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Info cupon Preview");
            var Result = await _cuponeraService.GetInfoCuponPreview(parametersReciboDto);
            return Ok(Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GenerateCupon([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Generar Cupon");

            var Result = await _cuponeraService.GenerateCupon(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetInfoCuponera([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo info cupon");
            var Result = await _cuponeraService.GetInfoCuponera(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetInfoCuponeraDetail([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Info cupon detail");
            var Result = await _cuponeraService.GetInfoCuponeraDetail(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetInfoMovimiento([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo obtiene informacion del movimiento");

            var Result = await _cuponeraService.GetInfoMovimiento(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AnnulmentCupon([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo elimina cupon");

            var Result = await _cuponeraService.AnnulmentCupon(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> PrintCuponera([FromBody]PrintCupon parametersPrint)
        {
            _logger.LogInfo("Metodo Print cupon");

            var Result = await _cuponeraService.PrintCupon(parametersPrint);
            return Ok(Result);
        }

    }
}