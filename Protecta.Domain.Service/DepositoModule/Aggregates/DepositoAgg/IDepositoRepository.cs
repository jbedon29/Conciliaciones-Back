using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg
{
    public interface IDepositoRepository
    {
        Task<List<Deposito>> ListarDeposito(DatosConsultaDeposito deposito);
        Task<List<Deposito>> ListarDepositoExtornado(DatosConsultaDepositoExtorno deposito);
    }
}
