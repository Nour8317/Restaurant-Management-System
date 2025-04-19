using Restorant.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restorant.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbset;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbset = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            T entity = await _dbset.FindAsync(id);
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, QueryOption<T> options)
        {
            IQueryable<T> query = _dbset;

            foreach (var include in options.GetIncludes()) 
            { 
                query = query.Include(include);
            }
            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string primaryKeyName = key? .Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        }

        public async Task UpdateAsync(T entity)
        {
            

            _dbset.Update(entity);
            await _context.SaveChangesAsync();
            //return entity;
        }
    }
}
