using Northwind.Data.Domain.Model;
using System.Collections.Generic;

namespace Northwind.Business.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
    }
}
