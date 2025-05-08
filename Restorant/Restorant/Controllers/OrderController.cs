using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restorant.Data;
using Restorant.Models;

namespace Restorant.Controllers
{
    public class OrderController : Controller
    {
        readonly ApplicationDbContext _context;
        private Repository<Product> _Products;
        private Repository<Order> _Orders;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, Repository<Product> products, Repository<Order> orders, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _Products = products;
            _Orders = orders;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _Products.GetAllAsync()
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItem(int prodId, int prodQty)
        {
            var product = await _context.Products.FindAsync(prodId);
            if (product == null) 
            {
                return NotFound();
            }
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _Products.GetAllAsync()
            };
            var existingItem = model.OrderItems.FirstOrDefault(oi => oi.ProductId == prodId);
            if (existingItem != null)
            {
                existingItem.Quantity += prodQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    Quantity = prodQty,
                    ProductName = product.Name
                });
            }

            model.TotalAmount = model.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            HttpContext.Session.Set("OrderViewModel", model);

            return RedirectToAction("Create",model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if(model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }
            return View(model);
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PlaceOrder()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = _userManager.GetUserId(User)
            };
            foreach (var item in model.OrderItems)
            {
                order.OrderItems.Add(
                    new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,

                    });

            }
            await _Orders.CreateAsync(order);
            HttpContext.Session.Remove("OrderViewModel");
            return RedirectToAction("ViewOrders");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = _userManager.GetUserId(User);
            var userOrders = await _Orders.GetAllByIdAsync(userId,"UserId", new QueryOption<Order>
            {
                Includes = "OrderItems.Product"
            });
            return View(userOrders);
        }
    }

}
