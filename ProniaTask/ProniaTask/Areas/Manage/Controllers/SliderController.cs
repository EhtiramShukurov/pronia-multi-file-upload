using Microsoft.AspNetCore.Mvc;
using ProniaTask.DAL;
using ProniaTask.Models;
using ProniaTask.ViewModels;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SliderController : Controller
    {
        IWebHostEnvironment _env { get; }
        AppDbContext _context { get; }
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Slides.ToList());
        }
        public IActionResult Delete(int id)
        {
            Slide slide = _context.Slides.Find(id);
            if (slide is null) return NotFound();
            _context.Slides.Remove(slide);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            IFormFile file = slideVM.Image;
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
            string fileName = Guid.NewGuid()+ (file.FileName.Length >64 ? file.FileName.Substring(0,64):file.FileName);
            using (var stream = new FileStream(Path.Combine(_env.WebRootPath,"assets","images","slider", fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Slide slide = new Slide { Offer = slideVM.Offer,Title = slideVM.Title,Order = slideVM.Order,Desc=slideVM.Desc,ImageUrl = fileName};
            if (_context.Slides.Any(s=>s.Order == slide.Order))
            {
                ModelState.AddModelError("Order", "There is already a slide in this order.");
            }
            _context.Slides.Add(slide);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Slide slide = _context.Slides.Find(id);
            if (slide is null) return NotFound();
            return View(slide);
        }
        [HttpPost]
        public IActionResult Update(int? id, Slide slide)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id is null ||id==0|| id != slide.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            Slide anotherSlide= _context.Slides.FirstOrDefault(s=>s.Order == slide.Order);
            if (anotherSlide != null)
            {
                anotherSlide.Order = _context.Slides.Find(id).Order;
            }
            Slide exist = _context.Slides.Find(slide.Id);
            if (exist is null) return NotFound();
            exist.Offer = slide.Offer;
            exist.ImageUrl = slide.ImageUrl;
            exist.Title = slide.Title;
            exist.Desc = slide.Desc;
            exist.Order = slide.Order;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
