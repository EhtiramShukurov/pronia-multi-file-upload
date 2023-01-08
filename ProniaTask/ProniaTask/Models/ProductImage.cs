using System.ComponentModel.DataAnnotations;

namespace ProniaTask.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [StringLength(100), Required]
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public bool? IsCover { get; set; }
        public Product? Product { get; set; }

    }
}
