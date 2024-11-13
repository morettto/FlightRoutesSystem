using AutoMapper;
using FlightRoutesSystem.Application.Abstracts;
using FlightRoutesSystem.Application.Exceptions.Routes;
using FlightRoutesSystem.DataAccess.Repositories.Connections;
using FlightRoutesSystem.DataAccess.Repositories.Routes;
using FlightRoutesSystem.Domain.Entities.Connections;
using FlightRoutesSystem.Domain.Entities.Routes;
using FlightRoutesSystem.Domain.Entities.Routes.dto;
using FlightRoutesSystem.Validation.Routes;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace FlightRoutesSystem.Application.Services.Routes
{
    public class RouteService : BaseService<Route>
    {
        #region properties
        private IMapper _mapper;
        private ConnectionRepository _connectionRepository;
        private RouteRepository _routeRepository;
        private RouteValidator _validator;
        #endregion

        #region constructors
        public RouteService(RouteRepository repository, ConnectionRepository connectionRepository, IMapper mapper, RouteValidator validator) : base(repository)
        {
            _mapper = mapper;
            _connectionRepository = connectionRepository;
            _routeRepository = repository;
            _validator = validator;
        }
        #endregion

        #region public methods
        public Route MapAndAdd(RouteDTO entity)
        {
            Route route = _mapper.Map<RouteDTO, Route>(entity);

            ValidationResult validationResult = _validator.PublicValidate(route);

            Route routeAdded;

            if (validationResult.IsValid)
                routeAdded = base.Add(route);
            else
                throw new RouteValidationException(validationResult.Errors.FirstOrDefault().ErrorMessage);

            SetRouteConnections(routeAdded.Id, entity.AirportConnectionIds);

            return routeAdded;
        }

        public void MapAndUpdate(long routeId, RouteUpdateDTO entity)
        {
            Route routeInDb = base.GetById(routeId);

            if (routeInDb is null)
                throw new RouteNotFoundException();

            _connectionRepository.GetByRouteId(routeId).ForEach(connection => _connectionRepository.Remove(connection));

            SetRouteConnections(routeId, entity.AirportConnectionIds);

            routeInDb = _mapper.Map(entity, routeInDb);

            ValidationResult validationResult = _validator.PublicValidate(routeInDb);
            
            if (validationResult.IsValid)
                base.Update(routeInDb);
            else
                throw new RouteValidationException(validationResult.Errors.FirstOrDefault().ErrorMessage);
        }

        public Route GetCheapestRoute(long originId, long destinyId)
        {
            return _routeRepository.GetCheapestRoute(originId, destinyId);
        }

        public override List<Route> GetAll()
        {
            return _routeRepository.GetAllWithOriginAndDestinyAndConnections();
        }

        private void SetRouteConnections(long routeId, List<long> airportsConnectionIds)
        {
            airportsConnectionIds.ForEach(airportId =>
            {
                Connection connection = new Connection()
                {
                    AirportId = airportId,
                    RouteId = routeId
                };

                _connectionRepository.Add(connection);
            });
        }
        #endregion
    }
}
