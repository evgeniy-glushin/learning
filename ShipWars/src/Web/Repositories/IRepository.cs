using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repositories
{
    public interface IRepository<T> where T : Entity  
    {
        IQueryable<T> All { get; }
        Task<T> GetByIdAsync(int id);
        T Insert(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task<int> CommitAsync();
    }
}
