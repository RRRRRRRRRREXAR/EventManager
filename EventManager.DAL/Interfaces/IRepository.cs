using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Interfaces
{
    interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task Create(T item);
        Task Update(T item);
        Task Delete(int id);
    }
}
