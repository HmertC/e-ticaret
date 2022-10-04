using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Concrete.EFCore
{
    public class EfCoreGenericRepository<TEnt> : IRepository<TEnt> where TEnt : class 
    {
        protected readonly DbContext context;
        public EfCoreGenericRepository(DbContext ctx)
        {
            context = ctx;
        }
        public void Create(TEnt entity)
        {
                context.Set<TEnt>().Add(entity);
                
        }

        public void Delete(TEnt entity)
        {
                context.Set<TEnt>().Remove(entity);
               
        }

        public List<TEnt> GetAll()
        {   
                return context.Set<TEnt>().ToList();
        }
        public TEnt GetById(int id)
        {  
                return context.Set<TEnt>().Find(id);     
        }
        public virtual void Update(TEnt entity)
        {   
                context.Entry(entity).State = EntityState.Modified;
              
        }
    }
}
