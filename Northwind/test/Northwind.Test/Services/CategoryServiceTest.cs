using FluentAssertions;
using Moq;
using Northwind.Business.Interfaces;
using Northwind.Business.Services;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.UnitTest.Services
{
    class CategoryServiceTest
    {
        private ICategoryService _categoryService;
        private Mock<IRepository<Category>> _moqCategoryRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _moqCategoryRepository = new Mock<IRepository<Category>>();
        }

        [Test]
        public void GetAll_WhereCategoryExists_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryId = 0, CategoryName = "name0", Description = "decription0", Picture = new byte[] { byte.MinValue } },
                new Category { CategoryId = 1, CategoryName = "name1", Description = "decription1", Picture = new byte[] { byte.MinValue } },
                new Category { CategoryId = 2, CategoryName = "name2", Description = "decription2", Picture = new byte[] { byte.MinValue } },
                new Category { CategoryId = 3, CategoryName = "name3", Description = "decription3", Picture = new byte[] { byte.MinValue } },
                new Category { CategoryId = 4, CategoryName = "name4", Description = "decription4", Picture = new byte[] { byte.MinValue } },
            };

            _moqCategoryRepository.Setup(x => x.GetAll()).Returns(categories.AsQueryable());
            _categoryService = new CategoryService(_moqCategoryRepository.Object);

            // Act
            var result = _categoryService.GetAll();

            // Assert
            result.Should().BeEquivalentTo(categories);
        }

        [Test]
        public void GetAll_WhereCategoryDoesNotExists_ShouldReturnEmptyCollection()
        {
            // Arrange
            var categories = new List<Category>();

            _moqCategoryRepository.Setup(x => x.GetAll()).Returns(categories.AsQueryable());
            _categoryService = new CategoryService(_moqCategoryRepository.Object);

            // Act
            var result = _categoryService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
