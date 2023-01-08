using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProniaTask.DAL;
using ProniaTask.Models;
using ProniaTask.Utilities;
using ProniaTask.ViewModels;

namespace ProniaTask.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        AppDbContext _context { get; }
        IWebHostEnvironment _env { get; }
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Products?.Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color)
                .Include(p=>p.ProductCategories).ThenInclude(pc=>pc.Category).Include(p=>p.ProductSizes).ThenInclude(ps=>ps.Size));
        }
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {
            var coverImg = cp.CoverImage;
            var hoverImg = cp.HoverImage;
            var otherImgs = cp.OtherImages;
            if (coverImg?.CheckType("image/")== false)
            {
                ModelState.AddModelError("Cover Image","File you uploaded is not an image!");
            }
            if (coverImg?.CheckSize(3 * 1024) == false)
            {
                ModelState.AddModelError("Cover Image", "File you upload should be smaller than 3 mb!");
            }
            if (hoverImg?.CheckType("image/") == false)
            {
                ModelState.AddModelError("Hover Image", "File you uploaded is not an image!");
            }
            if (hoverImg?.CheckSize(3 * 1024) == false)
            {
                ModelState.AddModelError("Hover Image", "File you upload should be smaller than 3 mb!");
            }
            if (!ModelState.IsValid) 
            {
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View();
            }
            var sizes = _context.Sizes.Where(s => cp.SizeIds.Contains(s.Id));
            var colors = _context.Colors.Where(c => cp.ColorIds.Contains(c.Id));
            var categories = _context.Categories.Where(ca => cp.CategoryIds.Contains(ca.Id));
            Product product = new Product
            {
                Name = cp.Name,
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                Description = cp.Description,
                Discount = cp.Discount,
                IsDeleted = false,
                SKU = "1"
                
            };
            List<ProductImage> images = new List<ProductImage>();
            images.Add(
                new ProductImage
                {
                    ImageUrl = coverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")),
                    IsCover = true,
                    Product = product
                });
            images.Add(
                new ProductImage
                {
                    ImageUrl = hoverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")),
                    IsCover = false,
                    Product = product
                });
            foreach (var item in otherImgs)
            {
                if (item?.CheckType("image/")== false)
                {
                    ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                    ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                    ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                    ModelState.AddModelError("OtherImages", "File you uploaded is not an image!");
                    return View();
                }
                if (item?.CheckSize(3 * 1024)== false)
                {
                    ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                    ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                    ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                    ModelState.AddModelError("OtherImages", "File you upload should be smaller than 3 mb!");
                    return View();
                }
                images.Add(
                    new ProductImage
                    {
                        ImageUrl = item.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")),
                        IsCover = null,
                        Product = product
                    });
            }
            product.ProductImages = images;
            _context.Products.Add(product);
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor { Product = product, ColorId = item.Id });
            }
            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize { Product = product, SizeId = item.Id });
            }
            foreach (var item in categories)
            {
                _context.ProductCategories.Add(new ProductCategory { Product = product, CategoryId = item.Id });
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            return View(product);
        }
        [HttpPost]
        public IActionResult Update(int? id, Product product)
        {
            if (!ModelState.IsValid) 
            {
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View();
            }
            if (id is null || id != product.Id) return BadRequest();
            Product exist = _context.Products.Find(id);
            if (exist is null) return NotFound();
            exist.Name = product.Name;
            exist.SKU = product.SKU;
            exist.Description = product.Description;
            exist.CostPrice = product.CostPrice;
            exist.SellPrice = product.SellPrice;
            exist.Discount = product.Discount;


            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}
