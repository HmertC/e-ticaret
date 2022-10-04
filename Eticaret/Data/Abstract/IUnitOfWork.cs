using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        void Save();

    }
}
