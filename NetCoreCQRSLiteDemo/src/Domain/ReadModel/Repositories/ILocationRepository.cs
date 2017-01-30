using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ReadModel.Repositories
{
    interface ILocationRepository : IBaseRepository<ReadModel.LocationRM>
    {
        // TODO wtf why is not this in IBaseRepository?
        IEnumerable<LocationRM> GetAll();
        IEnumerable<EmployeeRM> GetEmployees(int locationID);
        bool HasEmployee(int locationID, int employeeID);
    }
}
