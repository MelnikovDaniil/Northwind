using Northwind.Business.Interfaces;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Northwind.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _categoryRepository.GetAll();
            return categories;
        }
    }
}
