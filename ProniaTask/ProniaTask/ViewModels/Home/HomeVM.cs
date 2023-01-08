using ProniaTask.Models;

namespace ProniaTask.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slide> Slides { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Shipping> Shippings { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}
