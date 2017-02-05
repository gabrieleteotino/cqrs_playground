using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Domain.ReadModel.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(IConnectionMultiplexer redis) : base(redis, "employee") { }

        #region IEmployeeRepository
        public IEnumerable<EmployeeRM> GetAll()
        {
            return Get<List<EmployeeRM>>("all");
        }

        public EmployeeRM GetByID(int id)
        {
            return Get<EmployeeRM>(id);
        }
        public IEnumerable<EmployeeRM> GetMultiple(IEnumerable<int> ids)
        {
            return GetMultiple<EmployeeRM>(ids);
        }

        public void Save(EmployeeRM employee)
        {
            Save(employee.EmployeeID, employee);
            MergeIntoAllCollection(employee);
        }
        #endregion

        private void MergeIntoAllCollection(EmployeeRM employee)
        {
            List<EmployeeRM> allEmployees = new List<EmployeeRM>();
            if (Exists("all"))
            {
                allEmployees = Get<List<EmployeeRM>>("all");
            }

            //If the district already exists in the ALL collection, remove that entry
            if (allEmployees.Any(x => x.EmployeeID == employee.EmployeeID))
            {
                allEmployees.Remove(allEmployees.First(x => x.EmployeeID == employee.EmployeeID));
            }

            //Add the modified district to the ALL collection
            allEmployees.Add(employee);

            Save("all", allEmployees);
        }
    }
}
