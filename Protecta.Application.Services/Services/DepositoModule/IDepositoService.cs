using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Application.Service.Dtos.Deposito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Services.DepositoModule
{
    public interface IDepositoService
    {
        Task<IEnumerable<DepositoDto>> ConsultarDepositosPendientes(DatosConsultaDepositoDto datosConsultaDepositoDto);
        Task<IEnumerable<DepositoDto>> ConsultarDepositosExtornado(DatosConsultaDepositoExtDto datosConsultaDepositoDto);

    }
}
