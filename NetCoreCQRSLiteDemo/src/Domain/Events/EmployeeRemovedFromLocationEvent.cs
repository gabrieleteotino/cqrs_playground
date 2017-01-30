using System;
using CQRSlite.Events;

namespace Domain.Events
{
    internal class EmployeeRemovedFromLocationEvent : BaseEvent
    {
        public readonly int EmployeeID;
        public readonly int OldLocationID;

        public EmployeeRemovedFromLocationEvent(Guid id, int _locationID, int employeeID)
        {
            this.Id = id;
            this.OldLocationID = _locationID;
            this.EmployeeID = employeeID;
        }
    }
}