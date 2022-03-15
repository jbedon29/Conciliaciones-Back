using Protecta.Application.Service.Dtos.Moneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.MonedaService
{
    public class MonedaService :IMonedaService
    {
        public async Task<IEnumerable<MonedaDto>> ListarMoneda()
        {
            ////Traer datos de los productos de base de datos
            //var monedaResult = await _entidadRepository.ListarMoneda();

            ////Verifica que el resultado no sea nulo
            //if (monedaResult == null)
            //    return null;

            ////Mapeo
            //var MonedasDtos = _mapper.Map<IEnumerable<MonedaDto>>(monedaResult);

            var MonedasDtos = new List<MonedaDto>() {
                                    new MonedaDto(){ IdMoneda=1001, Codigo="PEN"},
                                    new MonedaDto(){ IdMoneda=1002, Codigo="USD"}
                                    };
            return MonedasDtos;
        }
    }
}
