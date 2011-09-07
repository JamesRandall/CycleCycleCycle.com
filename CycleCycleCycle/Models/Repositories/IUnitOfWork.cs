using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CycleCycleCycle.Models.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        void Save();
    }
}
