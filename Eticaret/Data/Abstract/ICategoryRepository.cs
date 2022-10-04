using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Category GetByIdWithProducts(int categoryId);

        void DeleteFromCategory(int producId, int categoryId);
    }
}
