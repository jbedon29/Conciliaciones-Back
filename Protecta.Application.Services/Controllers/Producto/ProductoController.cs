using System;
using AutoMapper;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Application.Service.Dtos.Producto;
using Protecta.Application.Service.Services.ProductoModule;

namespace Protecta.Application.Service.Controllers.Producto
{
    [Route("api/[controller]")]
    public class ProductoController : Controller
    {
        private IProductoService _ProductoService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public ProductoController(ILoggerManager logger, IProductoService _productoService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _ProductoService = _productoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")] 
        public async Task<IActionResult> ListarProducto()
        {
            _logger.LogInfo("Metodo Listar Producto");

            var productoResult = await _ProductoService.ListarProducto();
            return Ok(productoResult);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ListarProductoSCTR()
        {
            _logger.LogInfo("Metodo Listar Producto SCTR");

            var productoResult = await _ProductoService.ListarProductoSCTR();
            return Ok(productoResult);
        }
    }
}