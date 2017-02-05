using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CQRSlite.Commands;
using Domain.ReadModel.Repositories;
using Web.Commands.Requests;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Commands.Controllers
{
    [Route("locations")]
    public class LocationController : Controller
    {
        private IMapper _mapper;
        private ICommandSender _commandSender;
        private ILocationRepository _locationRepo;
        private IEmployeeRepository _employeeRepo;

        public LocationController(IMapper mapper, ICommandSender commandSender, ILocationRepository locationRepo, IEmployeeRepository employeeRepo)
        {
            _mapper = mapper;
            _commandSender = commandSender;
            _locationRepo = locationRepo;
            _employeeRepo = employeeRepo;
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Create(CreateLocationRequest request)
        {
            var command = _mapper.Map<Domain.Commands.CreateLocationCommand>(request);
            _commandSender.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("assignemployee")]
        public ActionResult AssignEmployee(AssignEmployeeToLocationRequest request)
        {
            var employee = _employeeRepo.GetByID(request.EmployeeID);
            if (employee.LocationID != 0)
            {
                var oldLocationAggregateID = _locationRepo.GetByID(employee.LocationID).AggregateID;

                var removeCommand = new Domain.Commands.RemoveEmployeeFromLocationCommand(oldLocationAggregateID, request.LocationID, employee.EmployeeID);
                _commandSender.Send(removeCommand);
            }

            var locationAggregateID = _locationRepo.GetByID(request.LocationID).AggregateID;
            var assignCommand = new Domain.Commands.AssignEmployeeToLocationCommand(locationAggregateID, request.LocationID, request.EmployeeID);
            _commandSender.Send(assignCommand);

            return Ok();
        }
    }
}
