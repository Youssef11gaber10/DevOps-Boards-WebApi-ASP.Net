using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;


namespace Talabat.Core.Repositories
{
    //public interface IGenericRepository<T> where T : class //instead of being class its being baseEntity the whole Models(Entities) inherts it to ensure which come is only model(enitiy)

    public interface IGenericRepository<T> where T : BaseEntity 
    {



        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync(string? SpecDEV, string? Status, string? Name, int? projectId);



        Task<T> CreateAsync(T Item);
        void Update(T Item);
        
        void Delete(T Item);

        T GetById(int id);




    }




    
}
