using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalShelter.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly AnimalShelterContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AnimalShelterContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
