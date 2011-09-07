using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CycleCycleCycle.Models.Repositories
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> All
        {
            get { return _context.Set<T>(); }
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = All;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void InsertOrUpdate(T entity, Func<T, int> idFunc)
        {
            if (idFunc(entity) == default(int))
            {
                // New entity
                _context.Set<T>().Add(entity);
            }
            else
            {
                // Existing entity
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            T entity = Find(id);
            _context.Set<T>().Remove(entity);
        }
    }
}