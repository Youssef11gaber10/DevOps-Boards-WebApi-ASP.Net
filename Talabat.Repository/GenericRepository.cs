using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }




        public async Task<T> CreateAsync(T Item)
        {
            await _storeContext.AddAsync(Item);//even project or task
            await _storeContext.SaveChangesAsync();//update database
            return Item;
        }

        public void Delete(T Item)
        {
            _storeContext.Remove(Item);
            _storeContext.SaveChanges();
        }


        public void Update(T Item)
        {
            _storeContext.Update(Item);
            _storeContext.SaveChanges();
        }



        #region GetAll
        public async Task<IReadOnlyList<T>> GetAllAsync(string? SpecDEV, string? Status, string? Name, int? projectId)
        {
            //this violate open/closed principle each time add another model need to modify here and add new if condition 
            if (typeof(T) == typeof(Product))//never take this func to get product will call it to get types and brands
                return (IReadOnlyList<T>)await _storeContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();


            if (typeof(T) == typeof(Project))
            {
                if (Name is not null)
                {
                    return (IReadOnlyList<T>)await _storeContext.Projects.Where(P => P.Name == Name).Include(P => P.Developers.Where(D => D.RoleName == "Developer"))
                      .Include(P => P.Tasks).ToListAsync();

                }

                if (projectId is not null)
                {
                    return (IReadOnlyList<T>)await _storeContext.Projects.Where(P => P.Id == projectId).Include(P => P.Developers.Where(D => D.RoleName == "Developer"))
                      .Include(P => P.Tasks).ToListAsync();

                }
                return (IReadOnlyList<T>)await _storeContext.Projects.Include(P => P.Developers.Where(D => D.RoleName == "Developer"))
                       .Include(P => P.Tasks).ToListAsync();

            }


            if (typeof(T) == typeof(Tasky))
            {
                if (SpecDEV is not null)
                {
                    return (IReadOnlyList<T>)await _storeContext.Tasks.Where(P => P.Developer.UserName == SpecDEV).Include(P => P.Comments)
                   .Include(P => P.Developer)
                   .Include(P => P.Project)
                   .ToListAsync();
                }
                if (Status is not null)
                {
                    return (IReadOnlyList<T>)await _storeContext.Tasks.Where(T => T.Status == Status).Include(P => P.Comments)
                   .Include(P => P.Developer)
                   .Include(P => P.Project)
                   .ToListAsync();
                }
                if (Name is not null)
                {
                    return (IReadOnlyList<T>)await _storeContext.Tasks.Where(T => T.Name == Name).Include(P => P.Comments)
                 .Include(P => P.Developer)
                 .Include(P => P.Project)
                 .ToListAsync();
                }

                return (IReadOnlyList<T>)await _storeContext.Tasks.Include(P => P.Comments)
                    .Include(P => P.Developer)
                    .Include(P => P.Project)
                    .ToListAsync();
            }



            return await _storeContext.Set<T>().ToListAsync();

        }

        #endregion


        #region GetById


        public async Task<T> GetByIdAsync(int id)
        {
            //return _storeContext.Set<T>().Where(t=>t.Id==id).FirstOrDefault();
            return await _storeContext.Set<T>().FindAsync(id);

            //return await _storeContext.Set<T>().Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType);

        }

        public T GetById(int id)
        {
            //return _storeContext.Set<T>().Where(t=>t.Id==id).FirstOrDefault();
            return _storeContext.Set<T>().Find(id);

            //return await _storeContext.Set<T>().Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType);

        }


        #endregion



    }
}
