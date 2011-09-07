using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CycleCycleCycle.Models.Repositories;

namespace CycleCycleCycle.Tests.Stubs
{
    class StubRepository<T> : IRepository<T> where T : class
    {


        public StubRepository()
        {
            FindIds = new List<int>();
            Items = new List<T>();
            InsertOrUpdateItems = new List<T>();
            InsertItems = new List<T>();
            UpdateItems = new List<T>();
            DeleteIds = new List<int>();
        }

        public List<int> FindIds { get; set; }
        public List<T> Items { get; set; }
        public List<T> InsertOrUpdateItems { get; set; }
        public List<T> InsertItems { get; set; }
        public List<T> UpdateItems { get; set; }
        public List<int> DeleteIds { get; set; } 

        public IQueryable<T> All
        {
            get { return Items.AsQueryable(); }
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return Items.AsQueryable();
        }

        public T Find(int id)
        {
            FindIds.Add(id);
            return Items[0];
        }

        public void InsertOrUpdate(T account, Func<T, int> idFunc)
        {
            InsertOrUpdateItems.Add(account);
        }

        public void Insert(T entity)
        {
            InsertItems.Add(entity);
        }

        public void Update(T entity)
        {
            UpdateItems.Add(entity);
        }

        public void Delete(int id)
        {
            DeleteIds.Add(id);
        }
    }
}
