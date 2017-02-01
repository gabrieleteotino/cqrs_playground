using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Events;
using AutoMapper;
using Domain.ReadModel.Repositories;
using Domain.ReadModel;

namespace Domain.EventHandlers
{
    public class LocationEventHandler : IEventHandler<LocationCreatedEvent>,
                                        IEventHandler<EmployeeAssignedToLocationEvent>,
                                        IEventHandler<EmployeeRemovedFromLocationEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILocationRepository _locationRepo;
        private readonly IEmployeeRepository _employeeRepo;

        public LocationEventHandler(IMapper mapper, ILocationRepository locationRepo, IEmployeeRepository employeeRepo)
        {
            _mapper = mapper;
            _locationRepo = locationRepo;
            _employeeRepo = employeeRepo;
        }

        public void Handle(LocationCreatedEvent message)
        {
            //Create a new LocationDTO object from the LocationCreatedEvent
            LocationRM location = _mapper.Map<LocationRM>(message);

            _locationRepo.Save(location);
        }

        public void Handle(EmployeeAssignedToLocationEvent message)
        {
            var location = _locationRepo.GetByID(message.NewLocationID);
            location.Employees.Add(message.EmployeeID);
            _locationRepo.Save(location);

            var employee = _employeeRepo.GetByID(message.EmployeeID);
            employee.LocationID = message.NewLocationID;
            _employeeRepo.Save(employee);
        }

        public void Handle(EmployeeRemovedFromLocationEvent message)
        {
            var location = _locationRepo.GetByID(message.OldLocationID);
            location.Employees.Remove(message.EmployeeID);
            _locationRepo.Save(location);
        }
    }
}
