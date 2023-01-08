using System.ComponentModel.DataAnnotations;

namespace ProniaTask.Models
{
    public class Client
    {
        public int Id { get; set; }

        [StringLength(20), Required]
        public string Name { get; set; }

        [StringLength(25), Required]
        public string Surname { get; set; }

        [StringLength(100), Required]
        public string ImageUrl { get; set; }

        [StringLength(30), Required]
        public string Occupation { get; set; }

        [StringLength(300),Required]
        public string Comment { get; set; }
    }
}
