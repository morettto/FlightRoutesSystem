using AutoMapper;
using FlightRoutesSystem.Application.Services.Airports;
using FlightRoutesSystem.Application.Services.Connections;
using FlightRoutesSystem.Application.Services.Routes;
using FlightRoutesSystem.DataAccess.Abstracts;
using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.DataAccess.Repositories.Airports;
using FlightRoutesSystem.DataAccess.Repositories.Connections;
using FlightRoutesSystem.DataAccess.Repositories.Routes;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Domain.Entities.Connections;
using FlightRoutesSystem.Domain.Entities.Routes;
using FlightRoutesSystem.Mapper;
using FlightRoutesSystem.Mapper.AirportProfile;
using FlightRoutesSystem.Validation.Airports;
using FlightRoutesSystem.Validation.Connections;
using FlightRoutesSystem.Validation.Routes;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FlightRoutesSystem.Api.Extensions.Dependencies
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencyInjection(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<FlightRoutesSystemContext>();

            List<Profile> profiles = new List<Profile>();
            profiles.Add(new AirportProfile());
            profiles.Add(new RouteProfile());
            var profileConfiguration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            IMapper mapper = new AutoMapper.Mapper(profileConfiguration);
            services.AddSingleton(mapper);

            services.AddScoped<BaseRepository<Airport>, AirportRepository>();
            services.AddScoped<BaseRepository<Connection>, ConnectionRepository>();
            services.AddScoped<BaseRepository<Route>, RouteRepository>();

            services.AddScoped<AirportRepository>();
            services.AddScoped<ConnectionRepository>();
            services.AddScoped<RouteRepository>();

            services.AddScoped<AirportService>();
            services.AddScoped<RouteService>();
            services.AddScoped<ConnectionService>();

            services.AddValidatorsFromAssemblyContaining<AirportValidator>();
            services.AddValidatorsFromAssemblyContaining<ConnectionValidator>();
            services.AddValidatorsFromAssemblyContaining<RouteValidator>();
        }
    }
}
