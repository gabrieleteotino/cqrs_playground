using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Domain.ReadModel.Repositories
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(IConnectionMultiplexer redis) : base(redis, "location") { }

        #region ILocationRepository
        public LocationRM GetByID(int locationID)
        {
            return Get<LocationRM>(locationID);
        }
        public IEnumerable<LocationRM> GetMultiple(IEnumerable<int> locationIDs)
        {
            return GetMultiple(locationIDs);
        }

        public IEnumerable<LocationRM> GetAll()
        {
            return Get<List<LocationRM>>("all");
        }

        public IEnumerable<EmployeeRM> GetEmployees(int locationID)
        {
            return Get<List<EmployeeRM>>(locationID.ToString() + ":employees");
        }

        public bool HasEmployee(int locationID, int employeeID)
        {
            //Deserialize the LocationDTO with the key location:{locationID}
            var location = Get<LocationRM>(locationID);

            //If that location has the specified Employee, return true
            return location.Employees.Contains(employeeID);
        }

        public void Save(LocationRM location)
        {
            Save(location.LocationID, location);
            MergeIntoAllCollection(location);
        }
        #endregion

        private void MergeIntoAllCollection(LocationRM location)
        {
            List<LocationRM> allLocations = new List<LocationRM>();
            if (Exists("all"))
            {
                allLocations = Get<List<LocationRM>>("all");
            }

            //If the district already exists in the ALL collection, remove that entry
            if (allLocations.Any(x => x.LocationID == location.LocationID))
            {
                allLocations.Remove(allLocations.First(x => x.LocationID == location.LocationID));
            }

            //Add the modified district to the ALL collection
            allLocations.Add(location);

            Save("all", allLocations);
        }
    }
}
