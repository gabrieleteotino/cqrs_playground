using System;
using CQRSlite.Events;

namespace Domain.Events
{
    internal class EmployeeAssignedToLocationEvent : BaseEvent
    {
        public readonly int EmployeeID;
        public readonly int NewLocationID;

        public EmployeeAssignedToLocationEvent(Guid id, int _locationID, int employeeID)
        {
            this.Id = id;
            this.NewLocationID = _locationID;
            this.EmployeeID = employeeID;
        }
    }
}