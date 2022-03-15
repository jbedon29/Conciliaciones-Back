using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Protecta.Domain.Service.ProductoModule.Aggregates.ProductoAgg
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ListarProducto();
        Task<List<Producto>> ListarProductoSCTR();
    }
}
