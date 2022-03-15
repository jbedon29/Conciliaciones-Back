using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Services.DepositoModule;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Controllers.Deposito
{
    [Route("api/[controller]")]
    public class DepositoController : Controller
    {
        private IDepositoService _DepositoService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public DepositoController(ILoggerManager logger, IDepositoService _depositoService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _DepositoService = _depositoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]        
        public async Task<IActionResult> ConsultarDepositosPendientes([FromBody] DatosConsultaDepositoDto datosConsultaDepositoDto)
        //public async Task<IActionResult> ConsultarDepositosPendientes()
        {
            _logger.LogInfo("Metodo Listar Depositos pendientes");

            //DatosConsultaDepositoDto datosConsultaDepositoDto = new DatosConsultaDepositoDto();
            //datosConsultaDepositoDto.IdProducto = 10001;
            //datosConsultaDepositoDto.FechaDesde = DateTime.Now.ToShortDateString();
            //datosConsultaDepositoDto.FechaHasta = DateTime.Now.ToShortDateString();
            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaDepositoDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaDepositoDto.FechaHasta);
            datosConsultaDepositoDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaDepositoDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var planillaResult = await _DepositoService.ConsultarDepositosPendientes(datosConsultaDepositoDto);
            return Ok(planillaResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConsultarDepositosExtornados([FromBody] DatosConsultaDepositoExtDto datosConsultaDepositoDto)        
        {
            _logger.LogInfo("Metodo Listar Depositos extornados");            

            var planillaResult = await _DepositoService.ConsultarDepositosExtornado(datosConsultaDepositoDto);
            return Ok(planillaResult);
        }

        
    }
}
