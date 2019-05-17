using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL.Interfaces
{
    //to do more service interfaces(((
    interface IService<T> where T :class
    {
        void Make(T item);
        void Update(T item);
        void Delete(T item);
        T GetItem(int? id);
        IEnumerable<T> GetPhones();
        IEnumerable<T> Find(Func<T,bool> predicate);
        void Dispose();
    }
}
