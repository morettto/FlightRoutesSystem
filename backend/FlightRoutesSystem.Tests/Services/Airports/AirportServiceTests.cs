using FlightRoutesSystem.Application.Exceptions.Airports;
using FlightRoutesSystem.Application.Services.Airports;
using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.DataAccess.Repositories.Airports;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Validation.Airports;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace FlightRoutesSystem.Tests.Services.Airports
{
    public class AirportServiceTests
    {
        private readonly Mock<FlightRoutesSystemContext> _contextMock;
        private readonly Mock<AirportRepository> _mockRepository;
        private readonly Mock<AirportValidator> _mockValidator;
        private readonly AirportService _service;

        public AirportServiceTests()
        {
            _contextMock = new Mock<FlightRoutesSystemContext>();
            _mockRepository = new Mock<AirportRepository>(_contextMock.Object);
            _mockValidator = new Mock<AirportValidator>();
            _service = new AirportService(_mockRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public void Add_WhenValidationSucceeds_ShouldAddAirport()
        {
            // Arrange
            var airport = new Airport { Id = 1, Name = "Test Airport" };
            var validationResult = new ValidationResult();

            _mockValidator
                .Setup(v => v.PublicValidade(It.IsAny<Airport>()))
                .Returns(validationResult);

            _mockRepository
                .Setup(r => r.Add(It.IsAny<Airport>()))
                .Returns(airport);

            // Act
            var result = _service.Add(airport);

            // Assert
            Assert.Equal(airport, result);
            _mockRepository.Verify(r => r.Add(airport), Times.Once);
            _mockValidator.Verify(r => r.PublicValidade(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void Add_WhenValidationFails_ShouldThrowAirportValidationException()
        {
            // Arrange
            var airport = new Airport { Id = 1, Name = "" };
            var validationFailure = new ValidationFailure("Name", "Name is required");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockValidator
                .Setup(v => v.PublicValidade(It.IsAny<Airport>()))
                .Returns(validationResult);

            // Act & Assert
            var exception = Assert.Throws<AirportValidationException>(() => _service.Add(airport));
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockRepository.Verify(r => r.Add(It.IsAny<Airport>()), Times.Never);
            _mockValidator.Verify(r => r.PublicValidade(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void GetById_ShouldReturnAirport()
        {
            // Arrange
            var airport = new Airport { Id = 1, Name = "Test Airport" };
            _mockRepository
                .Setup(r => r.GetById(1))
                .Returns(airport);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Equal(airport, result);
            _mockRepository.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void GetById_WhenAirportDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetById(It.IsAny<long>()))
                .Returns((Airport)null);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnAllAirports()
        {
            // Arrange
            var airports = new List<Airport>
            {
                new Airport { Id = 1, Name = "Airport 1" },
                new Airport { Id = 2, Name = "Airport 2" }
            };

            _mockRepository
                .Setup(r => r.GetAll())
                .Returns(airports);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(airports, result);
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenNoAirports_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetAll())
                .Returns(new List<Airport>());

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void Remove_ShouldRemoveAirport()
        {
            // Arrange
            var airport = new Airport { Id = 1, Name = "Test Airport" };

            // Act
            _service.Remove(airport);

            // Assert
            _mockRepository.Verify(r => r.Remove(airport), Times.Once);
        }

        [Fact]
        public void Update_ShouldUpdateAirport()
        {
            // Arrange
            var airport = new Airport { Id = 1, Name = "Updated Airport" };

            // Act
            _service.Update(airport);

            // Assert
            _mockRepository.Verify(r => r.Update(airport), Times.Once);
        }

        [Fact]
        public void Add_WithNullAirport_ShouldThrowValidationException()
        {
            // Arrange
            Airport airport = null;
            var validationFailure = new ValidationFailure("Airport", "Airport cannot be null");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockValidator
                .Setup(v => v.PublicValidade(It.IsAny<Airport>()))
                .Returns(validationResult);

            // Act & Assert
            var exception = Assert.Throws<AirportValidationException>(() => _service.Add(airport));
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockRepository.Verify(r => r.Add(It.IsAny<Airport>()), Times.Never);
            _mockValidator.Verify(r => r.PublicValidade(It.IsAny<Airport>()), Times.Once);
        }
    }
}
