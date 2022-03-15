using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos.Producto;

namespace Protecta.Application.Service.Services.ProductoModule
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> ListarProducto();
        Task<IEnumerable<ProductoDto>> ListarProductoSCTR();
    }
}
