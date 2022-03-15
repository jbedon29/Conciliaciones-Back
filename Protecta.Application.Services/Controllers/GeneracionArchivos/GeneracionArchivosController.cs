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
using Protecta.Application.Service.Services.GeneracionArchivosModule;
using Protecta.Application.Service.Dtos.GeneracionArchivos;

namespace Protecta.Application.Service.Controllers.GeneracionExactus
{
    [Route("api/[controller]")]
    public class GeneracionArchivosController : Controller
    {
        private IGeneracionArchivosService _service;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public GeneracionArchivosController(
            IGeneracionArchivosService service,
            ILoggerManager logger, 
            IMapper mapper, 
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> FechaGeneracion()
        {
            _logger.LogInfo("Metodo Fecha Generacion");

            var respuesta = await _service.FechaGeneracion();

            return Ok(respuesta);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GenerarArchivos([FromBody] DatosConsultaGeneracionArchivoDto datosConsultaArchivosDto)
        {
            _logger.LogInfo("Metodo Generar Archivos");

            var respuesta = await _service.ConsultarArchivos(datosConsultaArchivosDto);

            return Ok(respuesta);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ProcesarArchivos([FromBody] DatosConsultaGeneracionArchivoDto datosConsultaArchivosDto)
        {
            _logger.LogInfo("Metodo Procesar Archivos");

            var respuesta = await _service.ProcesarArchivos(datosConsultaArchivosDto);

            return Ok(respuesta);
        }
    }
}