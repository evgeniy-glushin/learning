using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web.Data;
using System.Linq;

namespace Web.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        ShipWarsDbContext _context;

        public Repository(ShipWarsDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> All => _context.Set<T>();

        public void Delete(T entity)
        {
            _context.Remove<T>(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public T Insert(T entity)
        {
           return _context.Add(entity)?.Entity;
        }

        public T Update(T entity)
        {
            return _context.Update<T>(entity).Entity; 
        }

        public async Task<int> CommitAsync () => await _context.SaveChangesAsync();
    }
}
