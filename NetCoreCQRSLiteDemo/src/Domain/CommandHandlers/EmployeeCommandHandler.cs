using CQRSlite.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Messages;
using Domain.Commands;
using CQRSlite.Domain;

namespace Domain.CommandHandlers
{
    public class EmployeeCommandHandler : ICommandHandler<Commands.CreateEmployeeCommand>
    {
        private ISession _session;
        public EmployeeCommandHandler(ISession session)
        {
            _session = session;
        }

        #region ICommandHandler
        public void Handle(CreateEmployeeCommand command)
        {
            WriteModel.Employee employee = new WriteModel.Employee(command.Id, command.EmployeeID, command.FirstName, command.LastName, command.DateOfBirth, command.JobTitle);
            _session.Add(employee);
            _session.Commit();
        } 
        #endregion
    }
}
