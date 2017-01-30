using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ReadModel.Repositories
{
    interface IEmployeeRepository : IBaseRepository<ReadModel.EmployeeRM>
    {
        // TODO wtf why is not this in IBaseRepository?
        IEnumerable<EmployeeRM> GetAll();
    }
}
