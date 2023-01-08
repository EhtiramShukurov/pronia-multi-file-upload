using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaTask.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [StringLength(100),Required]
        public string ImageUrl { get; set; }
    }
}
