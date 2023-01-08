using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaTask.ViewModels
{
    public class CreateSlideVM
    {
        [StringLength(20), Required]
        public string Offer { get; set; }

        [StringLength(30), Required]
        public string Title { get; set; }

        [StringLength(500), Required]
        public string Desc { get; set; }
        public int Order { get; set; }
        public IFormFile Image { get; set; }

    }
}
