using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class RemoveEmployeeFromLocationCommand : BaseCommand
    {
        // TODO (vedi todo in LocationCommandHandler) credo questi debbano diventare dei guid
        public readonly int EmployeeId;
        public readonly int LocationId;

        public RemoveEmployeeFromLocationCommand(Guid id, int employeeId, int locationId)
        {
            Id = id;
            EmployeeId = employeeId;
            LocationId = locationId;
        }
    }
}
