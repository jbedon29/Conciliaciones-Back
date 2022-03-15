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
using Protecta.Application.Service.Services.FacturaModule;

namespace Protecta.Application.Service.Controllers.Factura
{
    [Route("api/[controller]")]
    public class FacturaController : Controller
    {
        private IFacturaService _FacturaService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;       

        public FacturaController(ILoggerManager logger, IFacturaService _facturaService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _FacturaService = _facturaService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarDocumentosAbonos([FromBody] DatosConsultaDocumentoDto datosConsultaDocumentoDto) 
        {
            _logger.LogInfo("Método ListarDocumentosAbonos");

            var documentoAbonosResult = await _FacturaService.ListarDocumentosAbonos(datosConsultaDocumentoDto);

            return Ok(documentoAbonosResult);            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ValidarExisteFacturaDeposito()
        {
            _logger.LogInfo("Método ValidarExisteFacturaDeposito");

            var facturaDeposito = await _FacturaService.ValidarExisteFacturaDeposito();

            return Ok(facturaDeposito);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> GenerarFacturaAbonos([FromBody] DatosFacturaAbonosDto datosFacturaAbonos) 
        {
            _logger.LogInfo("Método GenerarFacturaAbonos");

            var generarFacturaResult = await _FacturaService.GenerarFacturaAbonos(datosFacturaAbonos);

            return Ok(generarFacturaResult);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GenerarNotaCredito()
        {
            _logger.LogInfo("Método GenerarNotaCredito");

            var generarNotaCredito = await _FacturaService.GenerarNotaCredito();

            return Ok(generarNotaCredito);
        }
    }
}