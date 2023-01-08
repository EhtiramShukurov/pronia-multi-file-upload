using System.ComponentModel.DataAnnotations;

namespace ProniaTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(20), Required]
        public string Name { get; set; }
        [Required]
        public string SKU { get; set; }
        //public int StockCount { get; set; }

        [Range(0,double.MaxValue)]
        public double CostPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double SellPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double Discount { get; set; }
        public bool IsDeleted { get; set; }

        [StringLength(500), Required]
        public string Description { get; set; }

        public ProductInformation? ProductInformation { get; set; }
        public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductSize>? ProductSizes { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}
