using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Perfil;
using Protecta.Application.Service.Services.PerfilModule;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;

namespace Protecta.Application.Service.Controllers.Perfil
{
    [Route("api/[controller]")]
    public class PerfilController : Controller
    {
        private IPerfilService _PerfilService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public PerfilController(ILoggerManager logger, IPerfilService _perfilService,
        IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _PerfilService = _perfilService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarTipoPerfil()
        {
            _logger.LogInfo("Metodo Listar tipoPerfil");

            var perfilResult = await _PerfilService.ListarTipoPerfil();
            return Ok(perfilResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarRecursoPerfil([FromBody]int idPerfil)
        {
            _logger.LogInfo("Metodo Listar ListarRecursoPerfil");

            var perfilResult = await _PerfilService.ListaRecursoPorPerfilAsync(idPerfil);
            return Ok(perfilResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConsultarPerfiles([FromBody] PerfilConsultaDto perfilConsultaDto)
        {
            _logger.LogInfo("Metodo Listar ConsultarPerfiles");
            var perfilResultDtos = await _PerfilService.ListarPerfiles(perfilConsultaDto);
            return Ok(perfilResultDtos);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarUsuarioPerfil([FromBody]int idPerfil)
        {
            var perfilResult = await _PerfilService.ListaUsuariosPorPerfil(idPerfil);
            return Ok(perfilResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegistrarPerfil([FromBody] RecursoProcesoDto recursoProcesoDto)
        {
            var registroResult = await _PerfilService.RegistroPerfilTask(recursoProcesoDto);
            return Ok(new { Mensaje = registroResult });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ActualizarPerfil([FromBody]RecursoProcesoDto recursoProcesoDto)
        {
            var actualizaResult = await _PerfilService.ActualizaPerfilTask(recursoProcesoDto);
            return Ok(new { Mensaje = actualizaResult });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody]RecursoProcesoDto recursoProcesoDto)
        {
            var eliminaResult = await _PerfilService.EliminaPerfilTask(recursoProcesoDto.IdPerfil, recursoProcesoDto.VcUsuariocreacion);
            return Ok(new { Mensaje = eliminaResult });
        }
    }
}