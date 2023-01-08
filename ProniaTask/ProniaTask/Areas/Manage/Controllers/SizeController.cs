using Microsoft.AspNetCore.Mvc;
using ProniaTask.DAL;
using ProniaTask.Models;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SizeController : Controller
    {
        AppDbContext _context { get; }
        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Sizes.ToList());
        }
        public IActionResult Delete(int id)
        {
            Size size = _context.Sizes.Find(id);
            if (size is null) return NotFound();
            _context.Sizes.Remove(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Size size = _context.Sizes.Find(id);
            if (size is null) return NotFound();
            return View(size);
        }
        [HttpPost]
        public IActionResult Update(int? id, Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id is null || id != size.Id) return BadRequest();
            Size exist = _context.Sizes.Find(id);
            if (exist is null) return NotFound();
            exist.Name = size.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
