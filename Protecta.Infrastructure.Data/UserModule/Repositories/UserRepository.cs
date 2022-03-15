using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;

namespace Protecta.Infrastructure.Data.UserModule.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<PRO_USER>> All()
        {
            List<PRO_USER> luser = new List<PRO_USER>();

            return luser;
        }

        public Task<string> Create(PRO_USER _dato)
        {
            throw new NotImplementedException();
        }

        public Task<PRO_USER> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Remove(PRO_USER _dato)
        {
            throw new NotImplementedException();
        }

        public Task<string> Update(PRO_USER _dato)
        {
            throw new NotImplementedException();
        }
    }
}
