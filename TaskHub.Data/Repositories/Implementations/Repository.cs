
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data.Models.Errors;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data.Repositories.Implementations {
    public class Repository<T> : IRepository<T> where T : class{
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DataContext dataContext){
            _context = dataContext;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new SavingErrorException($"error wile saving {typeof(T).Name} : {ex.Message}");
            }
        }


        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                T entity = await _dbSet.FirstOrDefaultAsync(predicate);
                if (entity == null)
                {
                    throw new NotFoundException($" {typeof(T).Name} not found");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new ReadErrorException($"Error while searching for entity of type {typeof(T).Name}: {ex.Message}");
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                T? entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new NotFoundException($"{typeof(T).Name} not found");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new ReadErrorException($"Error while reading entity of type {typeof(T).Name}: {ex.Message}");
            }
        }

        public async Task<T> UpdateAsync(Guid id, T entity)
        {
            T? _entity = await this.GetByIdAsync(id);
            if (_entity != null)
            {
                try
                {
                    var entityProperties = typeof(T).GetProperties();

                    foreach (var property in entityProperties)
                    {
                        if (property.CanWrite)
                        {
                            var newValue = property.GetValue(entity);
                            property.SetValue(_entity, newValue);
                        }
                    }

                    await _context.SaveChangesAsync();

                    return _entity;
                }
                catch (Exception ex)
                {
                   throw new UpdateErrorException($"Error while updating entity of type {typeof(T).Name}: {ex.Message}");
                }

            }
            else
            {
                throw new NotFoundException($"these user dosen't exists");
            }
        }

        public async void DeleteAsync(Guid id)
        {
            T? entity = await this.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"these {typeof(T).Name} dosen't exists");
            }
            else
            {
                try
                {
                     _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DeleteErrorException($"error while deleting {typeof(T).Name} : {ex.Message}");
                }
            }
        }
    } 
}