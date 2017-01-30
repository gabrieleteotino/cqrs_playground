using CQRSlite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.WriteModel
{
    public class Location : AggregateRoot
    {
        private int _locationID;
        private string _streetAddress;
        private string _city;
        private string _state;
        private string _postalCode;
        private IList<int> _employees;

        private Location() { }

        public Location(Guid id, int locationID, string streetAddress, string city, string state, string postalCode)
        {
            Id = id;
            _locationID = locationID;
            _streetAddress = streetAddress;
            _city = city;
            _state = state;
            _postalCode = postalCode;
            _employees = new List<int>();

            ApplyChange(new Events.LocationCreatedEvent(id, locationID, streetAddress, city, state, postalCode));
        }

        public void AddEmployee(int employeeID)
        {
            _employees.Add(employeeID);
            ApplyChange(new Events.EmployeeAssignedToLocationEvent(Id, _locationID, employeeID));
        }

        public void RemoveEmployee(int employeeID)
        {
            _employees.Remove(employeeID);
            ApplyChange(new Events.EmployeeRemovedFromLocationEvent(Id, _locationID, employeeID));
        }
    }
}
