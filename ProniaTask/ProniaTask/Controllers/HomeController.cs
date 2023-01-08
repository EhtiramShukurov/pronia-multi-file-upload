using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaTask.DAL;
using ProniaTask.Models;
using ProniaTask.ViewModels;

namespace ProniaTask.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context { get; }
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM home = new HomeVM { 
                Slides = _context.Slides.OrderBy(s=>s.Order), 
                Brands = _context.Brands,
                Clients = _context.Clients,
                Shippings = _context.Shippings,
                Products = _context.Products
            };
            ViewData["Banners"] = _context.Banners.ToList();
            return View(home);
        }
        public IActionResult Shop()
        {
            ViewBag.Categories = _context.Categories;
            ViewBag.Colors = _context.Colors;
            return View();
        }
        public IActionResult SingleProduct()
        {
            return View();
        }
        public IActionResult LoginRegister()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
    }
}
