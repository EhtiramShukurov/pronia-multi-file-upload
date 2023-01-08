using Microsoft.AspNetCore.Mvc;
using ProniaTask.DAL;
using ProniaTask.Models;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class ShippingController : Controller
    {
        AppDbContext _context { get; }
        public ShippingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Shippings.ToList());
        }
        public IActionResult Delete(int id)
        {
            Shipping shipping = _context.Shippings.Find(id);
            if (shipping is null) return NotFound();
            _context.Shippings.Remove(shipping);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewBag.count = _context.Shippings.Count();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Shippings.Add(shipping);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Shipping shipping = _context.Shippings.Find(id);
            if (shipping is null) return NotFound();
            return View(shipping);
        }
        [HttpPost]
        public IActionResult Update(int? id, Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id is null || id != shipping.Id) return BadRequest();
            Shipping exist = _context.Shippings.Find(id);
            if (exist is null) return NotFound();
            exist.LogoUrl = shipping.LogoUrl;
            exist.Title = shipping.Title;
            exist.Desc = shipping.Desc;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
