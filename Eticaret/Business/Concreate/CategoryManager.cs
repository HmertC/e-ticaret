using Business.Abstract;
using Data.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concreate
{
   public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitofwork;

        public CategoryManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Create(Category entity)
        {
            _unitofwork.Categories.Create(entity);
            _unitofwork.Save();
        }

        public void Delete(Category entity)
        {
            _unitofwork.Categories.Delete(entity);
            _unitofwork.Save();
        }

        public void DeleteFromCategory(int producId, int categoryId)
        {
            _unitofwork.Categories.DeleteFromCategory(producId,categoryId);
        }

        public List<Category> GetAll()
        {
            return _unitofwork.Categories.GetAll();
        }

        public Category GetById(int id)
        {
            return _unitofwork.Categories.GetById(id);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _unitofwork.Categories.GetByIdWithProducts(categoryId);
        }

        public void Update(Category entity)
        {
            _unitofwork.Categories.Update(entity);
            _unitofwork.Save();
        }

        public bool Validation(Category entity)
        {
            throw new NotImplementedException();
        }
     
    }
}
