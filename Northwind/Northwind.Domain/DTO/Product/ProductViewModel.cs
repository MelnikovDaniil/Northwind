using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Data.Domain.DTO.Product
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        [StringLength(20)]
        public string QuantityPerUnit { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
