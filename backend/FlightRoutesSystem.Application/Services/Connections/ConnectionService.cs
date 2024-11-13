using FlightRoutesSystem.Application.Abstracts;
using FlightRoutesSystem.DataAccess.Repositories.Connections;
using FlightRoutesSystem.Domain.Entities.Connections;

namespace FlightRoutesSystem.Application.Services.Connections
{
    public class ConnectionService : BaseService<Connection>
    {
        #region constructors
        public ConnectionService(ConnectionRepository repository) : base(repository)
        {
        }
        #endregion
    }
}
