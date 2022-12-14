using Entity;
using System.Collections.Generic;
using System.Linq;

namespace Eticaret.WebUI.Data
{
    public class CategoryRepository
    {
        private static List<Category> _categories=null;

        static CategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category {CategoryId=1,Name="Telefon"},
                new Category {CategoryId=2,Name="Bilgisayar"},
                new Category {CategoryId=3,Name="Elektronik"},
                new Category {CategoryId=4,Name="Kitap"}
            };
        }

        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public static void AddCategory(Category category)
        {
            _categories.Add(category);
        }

        public static Category GetCategorybyId(int id)
        {
            return _categories.FirstOrDefault(c=>c.CategoryId==id);
        }



    }
}