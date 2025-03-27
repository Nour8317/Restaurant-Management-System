using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restorant.Models;
using System;

namespace Restorant.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-Many Relationship Configuration
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            // Seed Data

            //// Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Pizza" },
                new Category { CategoryId = 2, Name = "Burgers"}
            );

            // Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Pepperoni Pizza", Description = "Spicy and cheesy", Price = 10.99M, Stock = 20, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Cheeseburger", Description = "Beef with cheese", Price = 8.99M, Stock = 15, CategoryId = 2 }
            );

            // Ingredients

            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient {IngredientId = 1, Name = "Cheese" },
                new Ingredient {IngredientId = 2, Name = "Pepperoni" },
                new Ingredient {IngredientId = 3, Name = "Beef Patty" }
            );


            // ProductIngredients (Many-to-Many)
            modelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 1 },  // Cheese for Pizza
                new ProductIngredient { ProductId = 1, IngredientId = 2 },  // Pepperoni for Pizza
                new ProductIngredient { ProductId = 2, IngredientId = 1 },  // Cheese for Burger
                new ProductIngredient { ProductId = 2, IngredientId = 3 }   // Beef Patty for Burger
            );

            //// Orders
            //modelBuilder.Entity<Order>().HasData(
            //    new Order { OrderId = 1, UserId = "user123", OrderDate = DateTime.UtcNow, TotalAmount = 19.98M }
            //);

            //// OrderItems
            //modelBuilder.Entity<OrderItem>().HasData(
            //    new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1, Quantity = 1, Price = 10.99M },
            //    new OrderItem { OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = 8.99M }
            //);
        }
    }
}
