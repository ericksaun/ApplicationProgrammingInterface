using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Repositories;
using Infrastructure.AppProgrammingInt.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.Repositories
{
    public class GenericAppProgrammingIntRepository<T> : IGenericAppProgrammingIntRepository<T> where T : class
    {
        protected readonly contextAppProgrammingInt _context;
        public GenericAppProgrammingIntRepository(contextAppProgrammingInt context)
        {
            _context = context;
        }
        
       

        public async Task AddAsync(T entity)
        {
            _context.Attach<T>(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Remove(T entity)
        {
            _context.Attach<T>(entity);
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public void Update(T entity)
        {
            _context.Attach<T>(entity);
            _context.Set<T>().Update(entity);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
    }
}
