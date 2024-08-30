using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Provide a Product Name")]
        [MaxLength(10, ErrorMessage = "Product Name can't be more than 10 characters")]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }
        public string Category { get; set; }

        [Range(1, 5, ErrorMessage = "StarRating can be within the range 1-5")]
        public double StarRating { get; set; }

        [AllowNull]
        public string Description { get; set; }

        public string ProductCode { get; set; }

        [AllowNull]
        public string ImageUrl { get; set; }
    }
}
