using Northwind.Data.Domain.Model;
using System.Collections.Generic;

namespace Northwind.Business.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();

        byte[] GetCategoryImage(int categoryId);

        bool UpdateCategoryImage(byte[] byteImage, int categoryId);

        Category Create(Category category);

        Category Update(int id, Category category);

        bool Delete(int categoryId);

    }
}
