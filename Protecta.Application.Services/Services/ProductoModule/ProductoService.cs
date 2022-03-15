using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Producto;
using Protecta.Domain.Service.ProductoModule.Aggregates.ProductoAgg;

namespace Protecta.Application.Service.Services.ProductoModule
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public ProductoService(IProductoRepository productoRepository, ILoggerManager logger, IMapper mapper)
        {
            this._productoRepository = productoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductoDto>> ListarProducto()
        {
            IEnumerable <ProductoDto> ProductoDtos = null;

            try
            {
                var productoResult = await _productoRepository.ListarProducto();

                if (productoResult == null) return null;

                ProductoDtos = _mapper.Map<IEnumerable<ProductoDto>>(productoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return ProductoDtos;
        }

        public async Task<IEnumerable<ProductoDto>> ListarProductoSCTR()
        {
            IEnumerable<ProductoDto> ProductoDtos = null;

            try
            {
                var productoResult = await _productoRepository.ListarProductoSCTR();

                if (productoResult == null) return null;

                ProductoDtos = _mapper.Map<IEnumerable<ProductoDto>>(productoResult);
            }
            catch (Exception ex)
            {
                ProductoDtos = new List<ProductoDto> { new ProductoDto { DescProducto = ex.Message } };
                // _logger.LogError(ex.InnerException.ToString());
            }

            return ProductoDtos;
        }
    }
}
