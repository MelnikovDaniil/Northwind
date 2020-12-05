using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Business.Interfaces;
using Northwind.Controllers;
using Northwind.Data.Domain.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.UnitTest.Controllers
{
    class CategoryControllerTest
    {
        private Mock<ICategoryService> _moqCategoryService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _moqCategoryService = new Mock<ICategoryService>();
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

            _moqCategoryService.Setup(x => x.GetAll()).Returns(categories.AsQueryable());
            var categoryController = new CategoryController(_moqCategoryService.Object);

            // Act
            var result = categoryController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            ((ViewResult)result).Model.Should().BeEquivalentTo(categories);
        }

        [Test]
        public void GetAll_WhereCategoryDoesNotExists_ShouldReturnEmptyCollection()
        {
            // Arrange
            var categories = new List<Category>();

            _moqCategoryService.Setup(x => x.GetAll()).Returns(categories.AsQueryable());
            var categoryController = new CategoryController(_moqCategoryService.Object);

            // Act
            var result = categoryController.Index();

            // Assert
            var subject = result.Should().BeOfType<ViewResult>().Subject;
            var resultCategories = (IEnumerable<Category>)subject.Model;
            resultCategories.Should().BeEmpty();
        }
    }
}
