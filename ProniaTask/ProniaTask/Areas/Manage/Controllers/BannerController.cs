using Microsoft.AspNetCore.Mvc;
using ProniaTask.DAL;
using ProniaTask.Models;
using ProniaTask.ViewModels;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class BannerController : Controller
    {
        AppDbContext _context { get; }
        public BannerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Banners.ToList());
        }
        public IActionResult Delete(int id)
        {
            Banner banner = _context.Banners.Find(id);
            if (banner is null) return NotFound();
            _context.Banners.Remove(banner);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewBag.Count = _context.Banners.Count();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBannerVM bannerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            IFormFile file = bannerVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "File you uploaded is not an image!");
                return View();
            }
            if (file.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "File you uploaded can't be larger than 2mb!");
                return View();
            }
            string fileName = Guid.NewGuid() + (file.FileName.Length > 64 ? file.FileName.Substring(0, 64) : file.FileName);
            using (var stream = new FileStream("C:\\Users\\User\\Desktop\\C#\\ProniaTask\\ProniaTask\\wwwroot\\assets\\images\\banner\\" + fileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Banner banner = new Banner { SecondaryTitle = bannerVM.SecondaryTitle, Title = bannerVM.Title, ImageUrl = fileName };
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Banner banner = _context.Banners.Find(id);
            if (banner is null) return NotFound();
            return View(banner);
        }
        [HttpPost]
        public IActionResult Update(int? id, Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id is null || id == 0 || id != banner.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            Banner exist = _context.Banners.Find(banner.Id);
            if (exist is null) return NotFound();
            exist.ImageUrl = banner.ImageUrl;
            exist.Title = banner.Title;
            exist.SecondaryTitle = banner.SecondaryTitle;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
