using System.ComponentModel.DataAnnotations;

namespace ProniaTask.Models
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(20), Required]
        public string Name { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }

    }
}
