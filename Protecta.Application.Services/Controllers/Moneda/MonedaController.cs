using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Services.MonedaService;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Controllers.Moneda
{
    [Route("api/[controller]")]
    public class MonedaController : Controller
    {
        private IMonedaService _MonedaService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public MonedaController(ILoggerManager logger, IMonedaService _monedaService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _MonedaService = _monedaService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarMoneda()
        //public async Task<IActionResult> ConsultarPlanillasProcesadas([FromBody] DatosConsultaPlanillaDto datosConsultaPlanillaDto)
        {
            _logger.LogInfo("Metodo Listar moneda");

            var planillaResult = await _MonedaService.ListarMoneda();
            return Ok(planillaResult);
        }
    }
}
