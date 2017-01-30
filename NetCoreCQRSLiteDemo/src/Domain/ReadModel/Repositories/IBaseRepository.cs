using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ReadModel.Repositories
{
    public interface IBaseRepository<T>
    {
        T GetByID(int id);
        IEnumerable<T> GetMultiple(IEnumerable<int> ids);
        bool Exists(int id);
        void Save(T item);
    }
}
