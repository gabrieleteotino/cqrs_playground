using AutoMapper;
using System;

namespace Web.Commands.AutoMapperProfiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Requests.CreateLocationRequest, Domain.Commands.CreateLocationCommand>()
                .ConstructUsing(x => new Domain.Commands.CreateLocationCommand(Guid.NewGuid(), x.LocationID, x.StreetAddress, x.City, x.State, x.PostalCode));

            CreateMap<Domain.Events.LocationCreatedEvent, Domain.ReadModel.LocationRM>()
                .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
