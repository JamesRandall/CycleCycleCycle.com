using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models.Repositories
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public EntityFrameworkUnitOfWork(IDbContextFactory dbContextFactory)
        {
            _context = dbContextFactory.CreateContext();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new EntityFrameworkRepository<T>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}