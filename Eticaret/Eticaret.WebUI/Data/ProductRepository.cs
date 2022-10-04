using System.Collections.Generic;
using System.Linq;
using Entity;


namespace Eticaret.WebUI.Data
{
    public static class ProductRepository
    {
        private static List<Product> _products = null;

        static ProductRepository()
        {
            _products = new List<Product>
            {
                new Product {ProductId=1,Name="Iphone 7",Price=3000,Description="iyi telefon",IsApproved=false, ImageUrl="1.jpg"},
                new Product {ProductId=2,Name="Iphone 8",Price=4000,Description="çok iyi telefon",IsApproved=true, ImageUrl="2.jpg"},
                new Product {ProductId=3,Name="Iphone X",Price=5000,Description="çok iyi telefon",IsApproved=true, ImageUrl="3.jpg"},
                new Product {ProductId=4,Name="Iphone 11",Price=7000,Description="çok iyi telefon", ImageUrl="4.jpg"},
                new Product {ProductId=5,Name="Iphone 12",Price=7000,Description="çok iyi telefon", ImageUrl="4.jpg"},
                new Product {ProductId=6,Name="Lenova 7",Price=3000,Description="iyi bilgisayar",IsApproved=false, ImageUrl="1.jpg"},
                new Product {ProductId=7,Name="Lenova 8",Price=4000,Description="çok iyi bilgisayar",IsApproved=true, ImageUrl="2.jpg"},
                new Product {ProductId=8,Name="Lenova X",Price=5000,Description="çok iyi bilgisayar",IsApproved=true, ImageUrl="3.jpg"},
                new Product {ProductId=9,Name="Lenova 11",Price=7000,Description="çok iyi bilgisayar", ImageUrl="4.jpg"}
            };
        }

        public static List<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public static void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public static Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.ProductId == id);
        }

        public static void EditProduct(Product product)
        {
            foreach (var p in _products)
            {
                if (p.ProductId == product.ProductId)
                {
                    p.Name = product.Name;
                    p.Price = product.Price;
                    p.ImageUrl = product.ImageUrl;
                    p.Description = product.Description;
                    p.IsApproved = product.IsApproved;
                   
                }
            }
        }

        public static void DeleteProduct(int id)
        {
            var product = GetProductById(id);

            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}