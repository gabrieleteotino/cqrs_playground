using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Commands.AutoMapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Requests.CreateEmployeeRequest, Domain.Commands.CreateEmployeeCommand>()
                .ConstructUsing(x => new Domain.Commands.CreateEmployeeCommand(Guid.NewGuid(), x.EmployeeID, x.FirstName, x.LastName, x.DateOfBirth, x.JobTitle));

            CreateMap<Domain.Events.EmployeeCreatedEvent, Domain.ReadModel.EmployeeRM>()
                .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
