﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Data.Domain.Model
{
    [Table("Categories")]
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName = "image")]
        [JsonIgnore]
        public byte[] Picture { get; set; }

        [InverseProperty("Category")]
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
