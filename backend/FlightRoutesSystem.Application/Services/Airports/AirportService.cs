using FlightRoutesSystem.Application.Abstracts;
using FlightRoutesSystem.Application.Exceptions.Airports;
using FlightRoutesSystem.DataAccess.Repositories.Airports;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Validation.Airports;
using FluentValidation.Results;
using System.Linq;

namespace FlightRoutesSystem.Application.Services.Airports
{
    public class AirportService : BaseService<Airport>
    {
        #region properties
        private AirportValidator _validator;
        #endregion

        #region constructors
        public AirportService(AirportRepository repository, AirportValidator validator) : base(repository)
        {
            _validator = validator;
        }
        #endregion

        #region overriders
        public override Airport Add(Airport entity)
        {
            ValidationResult validation = _validator.PublicValidade(entity);
            if (validation.IsValid)
                return base.Add(entity);
            else
                throw new AirportValidationException(validation.Errors.FirstOrDefault().ErrorMessage);
        }
        #endregion
    }
}
