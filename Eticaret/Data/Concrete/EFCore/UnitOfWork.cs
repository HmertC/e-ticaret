using Data.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Concrete.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext _context;
        public UnitOfWork(ShopContext context)
        {
            _context = context;
        }

        private EfCoreCartRepository _cartRepository;
        private EfCoreCategoryRepository _categoryRepository;
        private EfCoreProductRepository _productRepository;
        private EfCoreOrderRepository _orderRepository;

        public IProductRepository Products => _productRepository = _productRepository ?? new EfCoreProductRepository(_context);

        public ICartRepository Carts =>_cartRepository = _cartRepository ?? new EfCoreCartRepository(_context);

        public ICategoryRepository Categories => _categoryRepository = _categoryRepository ?? new EfCoreCategoryRepository(_context);

        public IOrderRepository Orders => _orderRepository = _orderRepository ?? new EfCoreOrderRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
