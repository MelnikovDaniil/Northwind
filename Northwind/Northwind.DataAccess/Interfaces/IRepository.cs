using System.Linq;

namespace Northwind.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Create new enitity in database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        T Create(T item);

        /// <summary>
        /// Find item at database and update them
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        T Update(T item);

        /// <summary>
        /// Remove item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// Get all entities from database
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Find element by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);
    }
}
