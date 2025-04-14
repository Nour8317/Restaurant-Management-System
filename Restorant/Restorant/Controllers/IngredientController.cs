using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restorant.Data;
using Restorant.Models;

namespace Restorant.Controllers
{
    public class IngredientController : Controller
    {
        private Repository<Ingredient> ingredients;
        private Repository<Product> products;
        public IngredientController(ApplicationDbContext context)
        {
            this.ingredients = new Repository < Ingredient>(context);
        }
        public async Task<IActionResult> Index()
        {
            return View(await ingredients.GetAllAsync());
        }
        public async Task<IActionResult> Details(int id)
        {

            return View(await ingredients.GetByIdAsync(id, new QueryOption<Ingredient>() { Includes="ProductIngredients.Product"}));
        }
        public async Task<IActionResult> Create()
        {
            //var productList = await products.GetAllAsync();
            //ViewBag.products = new SelectList(productList, "ProductId","Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientId", "Name")]Ingredient newIngredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.CreateAsync(newIngredient);
                return RedirectToAction("Index");
            }
            return View(newIngredient);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            return View(await ingredients.GetByIdAsync(Id, new QueryOption<Ingredient>() { Includes = "ProductIngredients.Product" }));
        }
        public async Task<IActionResult> DeleteConfirmed([Bind("IngredientId")] Ingredient ingredient)
        {
            if (ingredient == null)
                return NotFound();

            await ingredients.DeleteAsync(ingredient.IngredientId);
            return RedirectToAction("Index");
        }
    }
}
