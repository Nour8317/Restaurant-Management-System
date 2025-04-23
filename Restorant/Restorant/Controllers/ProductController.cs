using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restorant.Data;
using Restorant.Models;

namespace Restorant.Controllers
{
    public class ProductController : Controller
    {
        Repository<Product> products;
        Repository<Category> categories;
        public ProductController(ApplicationDbContext context) {
            this.products = new Repository<Product>(context);        
            this.categories = new Repository<Category>(context);        
        }
        public async Task<IActionResult> Index()
        { 
            return View(await products.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = new SelectList(await categories.GetAllAsync(), "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid) {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var fullPath = Path.Combine(wwwRootPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = fileName;
                }
                await products.CreateAsync(product);
                return RedirectToAction("Index");
        }
            ViewBag.categories = new SelectList(await categories.GetAllAsync(), "CategoryId", "Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await products.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("ProductId")] Product product)
        {
            var p = await products.GetByIdAsync(product.ProductId);
            if (p == null)
                return NotFound();

            if (!string.IsNullOrEmpty(p.ImageUrl) && p.ImageUrl != "default.jpg")
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", p.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await products.DeleteAsync(p.ProductId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.categories = new SelectList(await categories.GetAllAsync(), "CategoryId", "Name");
            return View(await products.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            await products.UpdateAsync(product);
            return RedirectToAction("Index");
        }

    }
}
