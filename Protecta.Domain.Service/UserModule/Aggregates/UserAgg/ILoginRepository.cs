using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using System.Collections.Generic;
using System.Threading.Tasks;
using Protecta.Domain.Interfaces.Repository.Common;
using Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg;
using Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg;

namespace Protecta.Domain.Service.UserModule.Aggregates.UserAgg
{
    public interface ILoginRepository : IRepository<Recursos>
    {
        Task<PRO_USER> Authenticate(string username, string password);
        Task<Respuesta> LecturaRecursos(string username);
    }
}


