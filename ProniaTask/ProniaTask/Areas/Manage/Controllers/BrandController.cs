using Microsoft.AspNetCore.Mvc;
using ProniaTask.DAL;
using ProniaTask.Models;
using ProniaTask.ViewModels;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController : Controller
    {
        AppDbContext _context { get; }
        IWebHostEnvironment _env { get; }
        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Brands.ToList());
        }
        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.Find(id);
            if (brand is null) return NotFound();
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBrandVM brandVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            IFormFile file = brandVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "File you uploaded is not an image!");
                return View();
            }
            if (file.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image you uploaded can't be larger than 2mb!");
                return View();
            }
            string fileName = Guid.NewGuid() + (file.FileName.Length > 64 ? file.FileName.Substring(0, 64) : file.FileName);
            using (var stream = new FileStream(Path.Combine(_env.WebRootPath,"assets","images","brand",fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Brand brand = new Brand { ImageUrl = fileName };
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Brand brand = _context.Brands.Find(id);
            if (brand is null) return NotFound();
            return View(brand);
        }
        [HttpPost]
        public IActionResult Update(int? id, Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id is null || id != brand.Id) return BadRequest();
            Brand exist = _context.Brands.Find(id);
            if (exist is null) return NotFound();
            exist.ImageUrl = brand.ImageUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
