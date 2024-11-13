using AutoMapper;
using FlightRoutesSystem.Domain.Entities.Routes;
using FlightRoutesSystem.Domain.Entities.Routes.dto;

namespace FlightRoutesSystem.Mapper
{
    public class RouteProfile : Profile
    {
        public RouteProfile()
        {
            CreateMap<RouteDTO, Route>();
            CreateMap<RouteUpdateDTO, Route>()
                    .ForMember(dest => dest.Id, opt => opt.DoNotUseDestinationValue());
        }
    }
}
