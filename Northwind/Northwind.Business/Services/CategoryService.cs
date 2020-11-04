using Northwind.Business.Interfaces;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using System;
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

        public byte[] GetCategoryImage(int categoryId)
        {
            var byteImage = _categoryRepository.GetById(categoryId).Picture;
            return byteImage;
        }

        public bool UpdateCategoryImage(byte[] byteImage, int categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);
            category.Picture = byteImage;

            var updatedCategory = _categoryRepository.Update(category);
            if (updatedCategory != null)
            {
                return true;
            }

            throw new Exception("Unable to update image for category");
        }
    }
}
