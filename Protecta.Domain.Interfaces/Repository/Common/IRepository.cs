using System.Collections.Generic;
using System.Threading.Tasks;

namespace Protecta.Domain.Interfaces.Repository.Common
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> All();
        Task<string> Create(TEntity _dato);
        Task<string> Update(TEntity _dato);
        Task<string> Remove(TEntity _dato);
    }
}
