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
        //------------Refactor Start
        [HttpPost("[action]")]
        public async Task<IActionResult> ValidateCouponBook([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Método valida cuponera para refinanciamiento");

            var Result = await _cuponeraService.ValidateCouponBook(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ValidateCoupon([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo valida cambios cupon");

            var Result = await _cuponeraService.ValidateCoupon(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RecalculateCouponBook([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo refinanciamiento preliminar cuponera");

            var Result = await _cuponeraService.RecalculateCouponBook(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefactorCoupon([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo refinanciamiento  cuponera");

            var Result = await _cuponeraService.RefactorCoupon(parametersReciboDto);
            return Ok(Result);
        }
        //------------Refactor End

        [HttpPost("[action]")]
        public async Task<IActionResult> PrintCuponera([FromBody]PrintCupon parametersPrint)
        {
            _logger.LogInfo("Metodo Print cupon");

            var Result = await _cuponeraService.PrintCupon(parametersPrint);
            return Ok(Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ValPrint([FromBody]PrintCupon parametersPrint)
        {
            _logger.LogInfo("Metodo Validar Print cupon");

            var Result = await _cuponeraService.ValidarPrintCupon(parametersPrint);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ValCantCoupons([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo valida cambios cupon");

            var Result = await _cuponeraService.ValCantCoupons(parametersReciboDto);
            return Ok(Result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ValCouponbook([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo valida cambios cupon");

            var Result = await _cuponeraService.ValCouponbook(parametersReciboDto);
            return Ok(Result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ListarRamos()
        {
            _logger.LogInfo("Metodo Listar Ramos Cup");

            var TransacionesResult = await _cuponeraService.ListRamos();
            return Ok(TransacionesResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListarProductos([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo Listar Productos Cup");

            var Result = await _cuponeraService.ListProductos(parametersReciboDto);
            return Ok(Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ValPolFact([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo Listar Productos Cup");

            var Result = await _cuponeraService.ValPolFact(parametersReciboDto);
            return Ok(Result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ListRecibos([FromBody]ParametersReciboDto parametersReciboDto)
        {
            _logger.LogInfo("Metodo Listar Productos Cup");

            var Result = await _cuponeraService.ListRecibos(parametersReciboDto);
            return Ok(Result);
        }
    }
}