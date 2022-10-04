using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
            new Product() { ProductId =1, Name = "Samsung M51", Url = "samsung-m51", Price = 5000, ImageUrl = "1.jpg", IsApproved = true },
            new Product() { ProductId =2, Name = "Redmi Note 11", Url = "redmi-note11", Price = 4000, ImageUrl = "2.jpg", IsApproved = false },
            new Product() { ProductId =3, Name = "Samsung M53", Url = "samsung-m53", Price = 5000, ImageUrl = "3.jpg", IsApproved = true },
            new Product() { ProductId =4, Name = "Iphone 13", Url = "apple-ıphone13", Price = 8000, ImageUrl = "4.jpg", IsApproved = false },
            new Product() { ProductId =5, Name = "Iphone 12", Url = "apple-iphone12", Price = 7000, ImageUrl = "5.jpg", IsApproved = true }
);
            builder.Entity<Category>().HasData(
              new Category() { CategoryId =1, Name = "Telefon", Url = "telefon" },
          new Category() { CategoryId =2, Name = "Bilgisayar", Url = "bilgisayar" },
          new Category() { CategoryId =3, Name = "Elektronik", Url = "elektronik" },
          new Category() { CategoryId =4, Name = "Doğal", Url = "dogal" });

            builder.Entity<ProductCategory>().HasData(
                      new ProductCategory() { ProductId =1, CategoryId =1 },
                new ProductCategory() { ProductId =1, CategoryId =2 },
                new ProductCategory() { ProductId =1, CategoryId =3 },
                new ProductCategory() { ProductId =2, CategoryId =1 },
                new ProductCategory() { ProductId =2, CategoryId =2 },
                new ProductCategory() { ProductId =2, CategoryId =3 },
                new ProductCategory() { ProductId =3, CategoryId =4 },
                new ProductCategory() { ProductId =4, CategoryId =3 },
                new ProductCategory() { ProductId =5, CategoryId =3 },
                new ProductCategory() { ProductId =5, CategoryId =1 }
             );
        }
    }
}
