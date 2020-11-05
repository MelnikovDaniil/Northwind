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
    class SupplierServiceTest
    {
        private ISupplierService _supplierService;
        private Mock<IRepository<Supplier>> _moqSupplierRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _moqSupplierRepository = new Mock<IRepository<Supplier>>();
        }

        [Test]
        public void GetAll_WhereCategoryExists_ShouldReturnAllCategories()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier { SupplierId = 0, CompanyName = "name0" },
                new Supplier { SupplierId = 1, CompanyName = "name1" },
                new Supplier { SupplierId = 2, CompanyName = "name2" },
                new Supplier { SupplierId = 3, CompanyName = "name3" },
                new Supplier { SupplierId = 4, CompanyName = "name4" },
            };

            _moqSupplierRepository.Setup(x => x.GetAll()).Returns(suppliers.AsQueryable());
            _supplierService = new SupplierService(_moqSupplierRepository.Object);

            // Act
            var result = _supplierService.GetAll();

            // Assert
            result.Should().BeEquivalentTo(suppliers);
        }

        [Test]
        public void GetAll_WhereCategoryDoesNotExists_ShouldReturnEmptyCollection()
        {
            // Arrange
            var suppliers = new List<Supplier>();

            _moqSupplierRepository.Setup(x => x.GetAll()).Returns(suppliers.AsQueryable());
            _supplierService = new SupplierService(_moqSupplierRepository.Object);

            // Act
            var result = _supplierService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
