using AutoMapper;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Domain.Entities.Airports.dto;

namespace FlightRoutesSystem.Mapper.AirportProfile
{
    public class AirportProfile : Profile
    {
        public AirportProfile()
        {
            CreateMap<AirportDTO, Airport>();
            CreateMap<AirportUpdateDTO, Airport>();
        }
    }
}
