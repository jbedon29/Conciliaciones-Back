using System;
using AutoMapper;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Application.Service.Dtos.Canal;
using Protecta.Application.Service.Services.CanalModule;

namespace Protecta.Application.Service.Controllers.Canal
{
    [Route("api/[controller]")]
    public class CanalController : Controller
    {
        private ICanalService _CanalService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public CanalController(ILoggerManager logger, ICanalService _canalService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _CanalService = _canalService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarCanal()
        {
            _logger.LogInfo("Metodo Listar Canal");

            var canalResult = await _CanalService.ListarCanal();
            return Ok(canalResult);
        }
    }
}
