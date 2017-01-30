using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class AssignEmployeeToLocationCommand : BaseCommand
    {
        // TODO (vedi todo in LocationCommandHandler) credo questi debbano diventare dei guid
        public readonly int EmployeeId;
        public readonly int LocationID;

        public AssignEmployeeToLocationCommand(Guid id, int locationID, int employeeID)
        {
            Id = id;
            EmployeeId = employeeID;
            LocationID = locationID;
        }
    }
}
