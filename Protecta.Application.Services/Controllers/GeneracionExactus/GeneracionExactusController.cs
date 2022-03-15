using System;
using AutoMapper;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Services.GeneracionExactusModule;

namespace Protecta.Application.Service.Controllers.GeneracionExactus
{
    [Route("api/[controller]")]
    public class GeneracionExactusController : Controller
    {
        private IGeneracionExactusService _GeneracionExactusService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public GeneracionExactusController(ILoggerManager logger, IGeneracionExactusService _generacionexactusService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _GeneracionExactusService = _generacionexactusService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GenerarArchivos([FromBody] DatosConsultaArchivosDto datosConsultaArchivosDto)
        {
            _logger.LogInfo("Metodo Generar Archivos");

            var generacionExactusResult = await _GeneracionExactusService.GenerarInterfaz(datosConsultaArchivosDto);

            return Ok(generacionExactusResult);
        }
    }
}