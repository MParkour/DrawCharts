using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Models.Utility;

namespace Models
{
    public interface IBase<T>
    {
        Message Add(T item);
        Message Update(T item);

        Message Remove(int id);
        Message Remove(T item);

        T Find(int id);
        IQueryable<T> GetAll();
    }
}
