using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaTask.Models
{
    public class Banner
    {
        public int Id { get; set; }

        [StringLength(20), Required]
        public string Title { get; set; }

        [StringLength(25), Required]
        public string SecondaryTitle { get; set; }

        [StringLength(100), Required]
        public string ImageUrl { get; set; }
    }
}
