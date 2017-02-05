using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CQRSlite.Commands;
using Web.Commands.Requests;
using Domain.ReadModel.Repositories;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Commands.Controllers
{
    [Route("employee")]
    public class EmployeeController : Controller
    {
        private IMapper _mapper;
        private ICommandSender _commandSender;
        private ILocationRepository _locationRepo;

        public EmployeeController(ICommandSender commandSender, IMapper mapper, ILocationRepository locationRepo)
        {
            _commandSender = commandSender;
            _mapper = mapper;
            _locationRepo = locationRepo;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreateEmployeeRequest request)
        {
            var command = _mapper.Map<Domain.Commands.CreateEmployeeCommand>(request);
            _commandSender.Send(command);

            // TODO I really dont like this, if the read model is not uptaded immediately the write model is not working
            // it's all nice and cool with the fluent validation of the request that stops this method from working id the location doesn't exist
            // but i still don't like it
            var locationAggregateID = _locationRepo.GetByID(request.LocationID).AggregateID;

            var assignCommand = new Domain.Commands.AssignEmployeeToLocationCommand(locationAggregateID, request.LocationID, request.EmployeeID);
            _commandSender.Send(assignCommand);
            return Ok();
        }
    }
}
