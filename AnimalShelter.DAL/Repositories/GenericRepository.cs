using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalShelter.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbContextFactory<AnimalShelterContext> _contextFactory;

        public GenericRepository(IDbContextFactory<AnimalShelterContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Task<List<T>> GetAllAsync() =>
            ExecuteAsync(context => context.Set<T>().ToListAsync());

        public Task<T?> GetByIdAsync(int id) =>
            ExecuteAsync(async context => await context.Set<T>().FindAsync(id));

        public Task AddAsync(T entity) =>
            ExecuteNonQueryAsync(async context =>
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
            });

        public Task UpdateAsync(T entity) =>
            ExecuteNonQueryAsync(async context =>
            {
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            });

        public Task DeleteAsync(int id) =>
            ExecuteNonQueryAsync(async context =>
            {
                var entity = await context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                }
            });

        private async Task<TResult> ExecuteAsync<TResult>(Func<AnimalShelterContext, Task<TResult>> action)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await action(context);
        }

        private async Task ExecuteNonQueryAsync(Func<AnimalShelterContext, Task> action)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await action(context);
        }
    }
}
