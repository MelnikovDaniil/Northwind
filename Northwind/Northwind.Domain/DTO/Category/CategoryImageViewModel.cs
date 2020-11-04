using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Data.Domain.DTO.Category
{
    public class CategoryImageViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile FormFile { get; set; }
    }
}
