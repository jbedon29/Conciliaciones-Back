using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Services.EntidadModule;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Controllers.Entidad
{
    [Route("api/[controller]")]
    public class EntidadController : Controller
    {
        private IEntidadService _EntidadService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public EntidadController(ILoggerManager logger, IEntidadService _entidadService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _EntidadService = _entidadService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarEntidad([FromBody]bool bTodos)        
        {
            _logger.LogInfo("Metodo Listar entidad");

            var planillaResult = await _EntidadService.ListarEntidades(bTodos);

            return Ok(planillaResult);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ListarCuentasxBanco([FromBody]int idBanco)        
        {
            _logger.LogInfo("Metodo Listar cuentas por banco");

            var planillaResult = await _EntidadService.ListarCuentaxEntidad(idBanco);
            return Ok(planillaResult);
        }

    }
}
