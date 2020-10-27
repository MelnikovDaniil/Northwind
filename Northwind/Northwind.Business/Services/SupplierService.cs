using Northwind.Business.Interfaces;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Northwind.Business.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository<Supplier> _supplierRepository;

        public SupplierService(IRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _supplierRepository.GetAll();
        }
    }
}
