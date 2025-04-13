using Microsoft.AspNetCore.Mvc;
using Restorant.Data;
using Restorant.Models;

namespace Restorant.Controllers
{
    public class IngredientController : Controller
    {
        private Repository<Ingredient> ingredients;
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
        public IActionResult Create()
        {
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
    }
}
