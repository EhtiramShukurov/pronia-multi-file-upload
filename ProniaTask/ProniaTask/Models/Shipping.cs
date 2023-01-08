using System.ComponentModel.DataAnnotations;

namespace ProniaTask.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        [StringLength(20), Required]
        public string Title { get; set; }
        [StringLength(100), Required]
        public string LogoUrl { get; set; }
        [StringLength(500), Required]
        public string Desc { get; set; }
    }
}
