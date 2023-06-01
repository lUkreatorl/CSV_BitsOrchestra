using CSV_Backend.Base;
using Microsoft.EntityFrameworkCore;

namespace CSV_Backend.Database
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity 
    {
        protected readonly ApplicationContext _context;

        public Repository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

            if (entity.Id != 0)
                throw new Exception("You can`t add entity with own ID.");
            
            await using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return entity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public async Task<List<TEntity>> AddRange(List<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), "Entity list cannot be null.");

            await using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.AddRangeAsync(entities);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return entities;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

            await using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<TEntity>().Update(entity);
                    _context.Entry(entity).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return entity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid ID.");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = await _context.Set<TEntity>().FindAsync(id);

                    if (entity == null)
                        throw new Exception($"Entity of type {typeof(TEntity).Name} with ID {id} not found.");

                    _context.Set<TEntity>().Remove(entity);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
