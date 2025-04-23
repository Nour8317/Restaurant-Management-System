using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restorant.Data;
using Restorant.Models;
using Restorant.ViewModels;

namespace Restorant.Controllers
{
    public class ProductController : Controller
    {
        Repository<Product> products;
        Repository<Category> categories;
        Repository<Ingredient> ingredients;
        public ProductController(ApplicationDbContext context) {
            this.products = new Repository<Product>(context);        
            this.categories = new Repository<Category>(context);
            this.ingredients = new Repository<Ingredient>(context);
        }
        public async Task<IActionResult> Index()
        { 
            return View(await products.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allIngredients = await ingredients.GetAllAsync();
            var viewModel = new ProductFormViewModel
            {
                Product = new Product(),
                Categories = (await categories.GetAllAsync())
                    .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                    .ToList(),
                Ingredients = allIngredients.Select(i => new IngredientCheckbox
                {
                    IngredientId = i.IngredientId,
                    Name = i.Name,
                    IsSelected = false
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var product = viewModel.Product;

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

                product.ProductIngredients = viewModel.Ingredients
                    .Where(i => i.IsSelected)
                    .Select(i => new ProductIngredient
                    {
                        IngredientId = i.IngredientId
                    }).ToList();

                await products.CreateAsync(product);
                return RedirectToAction("Index");
            }
            viewModel.Categories = (await categories.GetAllAsync())
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList();
            viewModel.Ingredients = (await ingredients.GetAllAsync())
                .Select(i => new IngredientCheckbox
                {
                    IngredientId = i.IngredientId,
                    Name = i.Name,
                    IsSelected = viewModel.Ingredients.Any(sel => sel.IngredientId == i.IngredientId && sel.IsSelected)
                }).ToList();

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await products.GetByIdAsync(id, new QueryOption<Product>{ Includes= "ProductIngredients.Ingredient" });
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("ProductId")] Product product)
        {
            var p = await products.GetByIdAsync(product.ProductId, new QueryOption<Product> { Includes = "ProductIngredients.Ingredient" });
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
            return View(await products.GetByIdAsync(id, new QueryOption<Product> { Includes = "ProductIngredients.Ingredient" }));
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
