using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService:IValidator<Product>
    {
        Product GetById(int id);
        Product GetByIdWithCategories(int id);
        List<Product> GetAll();
        Product GetProductDetails(string url);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);

        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        bool Create(Product entity);
        bool Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
      
    }
}
