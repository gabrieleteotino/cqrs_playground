using CQRSlite.Commands;
using CQRSlite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Commands;

namespace Domain.CommandHandlers
{
    public class LocationCommandHandler : 
        ICommandHandler<Commands.CreateLocationCommand>,
        ICommandHandler<Commands.AssignEmployeeToLocationCommand>,
        ICommandHandler<Commands.RemoveEmployeeFromLocationCommand>
    {
        private readonly ISession _session;

        public LocationCommandHandler(ISession session)
        {
            _session = session;
        }

        #region ICommandHandler<Commands.RemoveEmployeeFromLocationCommand>
        public void Handle(RemoveEmployeeFromLocationCommand command)
        {
            // TODO questo non dovrebbe riportare l'id della location?
            var location = _session.Get<WriteModel.Location>(command.Id);
            location.RemoveEmployee(command.EmployeeId);
            _session.Commit();
        } 
        #endregion

        #region ICommandHandler<Commands.AssignEmployeeToLocationCommand>
        public void Handle(AssignEmployeeToLocationCommand command)
        {
            // TODO questo non dovrebbe riportare l'id della location?
            var location = _session.Get<WriteModel.Location>(command.Id);
            location.AddEmployee(command.EmployeeId);
            _session.Commit();
        } 
        #endregion

        #region ICommandHandler<Commands.CreateLocationCommand>
        public void Handle(CreateLocationCommand command)
        {
            var location = new WriteModel.Location(command.Id, command.LocationID, command.StreetAddress, command.City, command.State, command.PostalCode);
            _session.Add(location);
            _session.Commit();
        }
        #endregion
    }
}
