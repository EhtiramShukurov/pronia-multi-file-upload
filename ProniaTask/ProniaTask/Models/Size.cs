using System.ComponentModel.DataAnnotations;

namespace ProniaTask.Models
{
    public class Size
    {
        public int Id { get; set; }
        [StringLength(20), Required]
        public string Name { get; set; }
        public ICollection<ProductSize>? ProductSizes { get; set; }
    }
}
