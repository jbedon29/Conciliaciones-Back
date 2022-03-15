using System;
using AutoMapper;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Application.Service.Dtos.TipoArchivo;
using Protecta.Application.Service.Services.TipoArchivoModule;

namespace Protecta.Application.Service.Controllers.TipoArchivo
{
    [Route("api/[controller]")]
    public class TipoArchivoController : Controller
    {
        private ITipoArchivoService _TipoArchivoService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public TipoArchivoController(ILoggerManager logger, ITipoArchivoService _tipoarchivoService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _TipoArchivoService = _tipoarchivoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarTipoArchivo()
        {
            _logger.LogInfo("Metodo Listar Tipo Archivo");

            var tipoarchivoResult = await _TipoArchivoService.ListarTipoArchivo();
            return Ok(tipoarchivoResult);
        }
    }
}