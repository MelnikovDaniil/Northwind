using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Data.Domain.DTO.Product
{
    public class ProductEditModel
    {
        public int? ProductId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "String must not include 40 characters")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "String must not include 20 characters")]
        public string QuantityPerUnit { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public IEnumerable<string> OtherCategories { get; set; }

        [Required]
        public string SupplierName { get; set; }

        public IEnumerable<string> OtherSuppliers { get; set; }

        [Range(0, 10000)]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
