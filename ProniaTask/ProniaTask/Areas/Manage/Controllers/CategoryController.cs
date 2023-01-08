using Microsoft.AspNetCore.Mvc;
using ProniaTask.DAL;
using ProniaTask.Models;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class CategoryController : Controller
    {
        AppDbContext _context { get; }
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Category category = _context.Categories.Find(id);
            if (category is null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int? id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id is null || id != category.Id) return BadRequest();
            Category exist = _context.Categories.Find(id);
            if (exist is null) return NotFound();
            exist.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
