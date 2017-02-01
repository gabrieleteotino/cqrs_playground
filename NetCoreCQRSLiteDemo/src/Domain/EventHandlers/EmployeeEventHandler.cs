using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Events;
using Domain.ReadModel.Repositories;
using AutoMapper;
using Domain.ReadModel;

namespace Domain.EventHandlers
{
    public class EmployeeEventHandler : IEventHandler<Events.EmployeeCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeEventHandler(IMapper mapper, IEmployeeRepository employeeRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
        }

        public void Handle(EmployeeCreatedEvent message)
        {
            EmployeeRM employee = _mapper.Map<EmployeeRM>(message);
            _employeeRepo.Save(employee);
        }
    }
}
