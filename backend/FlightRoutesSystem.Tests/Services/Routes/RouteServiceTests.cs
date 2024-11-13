using AutoMapper;
using FlightRoutesSystem.Application.Exceptions.Routes;
using FlightRoutesSystem.Application.Services.Routes;
using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.DataAccess.Repositories.Connections;
using FlightRoutesSystem.DataAccess.Repositories.Routes;
using FlightRoutesSystem.Domain.Entities.Connections;
using FlightRoutesSystem.Domain.Entities.Routes;
using FlightRoutesSystem.Domain.Entities.Routes.dto;
using FlightRoutesSystem.Validation.Routes;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace FlightRoutesSystem.Tests.Services.Routes
{
    public class RouteServiceTests
    {
        private readonly Mock<FlightRoutesSystemContext> _contextMock;
        private readonly Mock<RouteRepository> _routeRepositoryMock;
        private readonly Mock<ConnectionRepository> _connectionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<RouteValidator> _validatorMock;
        private readonly RouteService _service;

        public RouteServiceTests()
        {
            _contextMock = new Mock<FlightRoutesSystemContext>();
            _routeRepositoryMock = new Mock<RouteRepository>(_contextMock.Object);
            _connectionRepositoryMock = new Mock<ConnectionRepository>(_contextMock.Object);
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<RouteValidator>();

            _service = new RouteService(
                _routeRepositoryMock.Object,
                _connectionRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object
            );
        }

        [Fact]
        public void MapAndAdd_WhenValidationSucceeds_ShouldAddRouteAndConnections()
        {
            // Arrange
            var routeDto = new RouteDTO
            {
                AirportConnectionIds = new List<long> { 1, 2, 3 }
            };

            var route = new Route { Id = 1 };
            var validationResult = new ValidationResult();

            _mapperMock
                .Setup(m => m.Map<RouteDTO, Route>(It.IsAny<RouteDTO>()))
                .Returns(route);

            _validatorMock
                .Setup(v => v.PublicValidate(It.IsAny<Route>()))
                .Returns(validationResult);

            _routeRepositoryMock
                .Setup(r => r.Add(It.IsAny<Route>()))
                .Returns(route);

            // Act
            var result = _service.MapAndAdd(routeDto);

            // Assert
            Assert.Equal(route, result);
            _mapperMock.Verify(m => m.Map<RouteDTO, Route>(routeDto), Times.Once);
            _validatorMock.Verify(v => v.PublicValidate(route), Times.Once);
            _routeRepositoryMock.Verify(r => r.Add(route), Times.Once);
            _connectionRepositoryMock.Verify(r => r.Add(It.IsAny<Connection>()), Times.Exactly(3));
        }

        [Fact]
        public void MapAndAdd_WhenValidationFails_ShouldThrowRouteValidationException()
        {
            // Arrange
            var routeDto = new RouteDTO();
            var route = new Route();
            var validationFailure = new ValidationFailure("Property", "Error message");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mapperMock
                .Setup(m => m.Map<RouteDTO, Route>(It.IsAny<RouteDTO>()))
                .Returns(route);

            _validatorMock
                .Setup(v => v.PublicValidate(It.IsAny<Route>()))
                .Returns(validationResult);

            // Act & Assert
            var exception = Assert.Throws<RouteValidationException>(() => _service.MapAndAdd(routeDto));
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _routeRepositoryMock.Verify(r => r.Add(It.IsAny<Route>()), Times.Never);
            _connectionRepositoryMock.Verify(r => r.Add(It.IsAny<Connection>()), Times.Never);
        }

        [Fact]
        public void MapAndUpdate_WhenRouteExists_AndValidationSucceeds_ShouldUpdateRouteAndConnections()
        {
            // Arrange
            var routeId = 1L;
            var routeUpdateDto = new RouteUpdateDTO
            {
                AirportConnectionIds = new List<long> { 1, 2 }
            };
            var existingRoute = new Route { Id = routeId };
            var existingConnections = new List<Connection>
            {
                new Connection { Id = 1, RouteId = routeId }
            };
            var validationResult = new ValidationResult();

            _routeRepositoryMock
                .Setup(r => r.GetById(routeId))
                .Returns(existingRoute);

            _connectionRepositoryMock
                .Setup(r => r.GetByRouteId(routeId))
                .Returns(existingConnections);

            _validatorMock
                .Setup(v => v.PublicValidate(It.IsAny<Route>()))
                .Returns(validationResult);

            // Act
            _service.MapAndUpdate(routeId, routeUpdateDto);

            // Assert
            _connectionRepositoryMock.Verify(r => r.Remove(It.IsAny<Connection>()), Times.Once);
            _connectionRepositoryMock.Verify(r => r.Add(It.IsAny<Connection>()), Times.Exactly(2));
            _routeRepositoryMock.Verify(r => r.Update(It.IsAny<Route>()), Times.Once);
        }

        [Fact]
        public void MapAndUpdate_WhenRouteDoesNotExist_ShouldThrowRouteNotFoundException()
        {
            // Arrange
            var routeId = 1L;
            var routeUpdateDto = new RouteUpdateDTO();

            _routeRepositoryMock
                .Setup(r => r.GetById(routeId))
                .Returns((Route)null);

            // Act & Assert
            Assert.Throws<RouteNotFoundException>(() => _service.MapAndUpdate(routeId, routeUpdateDto));
            _connectionRepositoryMock.Verify(r => r.Remove(It.IsAny<Connection>()), Times.Never);
            _routeRepositoryMock.Verify(r => r.Update(It.IsAny<Route>()), Times.Never);
        }

        [Fact]
        public void MapAndUpdate_WhenValidationFails_ShouldThrowRouteValidationException()
        {
            // Arrange
            var routeId = 1L;
            var routeUpdateDto = new RouteUpdateDTO
            {
                AirportConnectionIds = new List<long> { 1, 2 }
            };
            var existingRoute = new Route { Id = routeId };
            var validationFailure = new ValidationFailure("Property", "Error message");
            var validationResult = new ValidationResult(new[] { validationFailure });

            var existingConnections = new List<Connection>
            {
                new Connection { Id = 1, RouteId = routeId }
            };

            _routeRepositoryMock
                .Setup(r => r.GetById(routeId))
                .Returns(existingRoute);

            _validatorMock
                .Setup(v => v.PublicValidate(It.IsAny<Route>()))
                .Returns(validationResult);

            _connectionRepositoryMock
               .Setup(r => r.GetByRouteId(routeId))
               .Returns(existingConnections);

            // Act & Assert
            var exception = Assert.Throws<RouteValidationException>(() =>
                _service.MapAndUpdate(routeId, routeUpdateDto));
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _routeRepositoryMock.Verify(r => r.Update(It.IsAny<Route>()), Times.Never);
        }

        [Fact]
        public void GetCheapestRoute_ShouldReturnRoute()
        {
            // Arrange
            var originId = 1L;
            var destinyId = 2L;
            var expectedRoute = new Route { Id = 1 };

            _routeRepositoryMock
                .Setup(r => r.GetCheapestRoute(originId, destinyId))
                .Returns(expectedRoute);

            // Act
            var result = _service.GetCheapestRoute(originId, destinyId);

            // Assert
            Assert.Equal(expectedRoute, result);
            _routeRepositoryMock.Verify(r => r.GetCheapestRoute(originId, destinyId), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllRoutesWithDetails()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route { Id = 1 },
                new Route { Id = 2 }
            };

            _routeRepositoryMock
                .Setup(r => r.GetAllWithOriginAndDestinyAndConnections())
                .Returns(routes);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(routes, result);
            Assert.Equal(2, result.Count);
            _routeRepositoryMock.Verify(r => r.GetAllWithOriginAndDestinyAndConnections(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenNoRoutes_ShouldReturnEmptyList()
        {
            // Arrange
            _routeRepositoryMock
                .Setup(r => r.GetAllWithOriginAndDestinyAndConnections())
                .Returns(new List<Route>());

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Empty(result);
            _routeRepositoryMock.Verify(r => r.GetAllWithOriginAndDestinyAndConnections(), Times.Once);
        }
    }
}