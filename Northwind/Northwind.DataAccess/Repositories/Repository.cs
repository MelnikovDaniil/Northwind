using Microsoft.EntityFrameworkCore;
using Northwind.Data.Domain;
using Northwind.DataAccess.Interfaces;
using System.Linq;

namespace Northwind.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly NorthwindContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public T Create(T item)
        {
            var createdItem = _dbSet.Add(item).Entity;
            _dbContext.SaveChanges();
            return createdItem;
        }

        public bool Delete(int id)
        {
            var deletedItem = _dbSet.Remove(_dbSet.Find(id));
            _dbContext.SaveChanges();
            if (deletedItem != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Update(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return item;
        }
    }
}
