using FlightRoutesSystem.DataAccess.Abstracts;
using FlightRoutesSystem.Domain.Base;
using System.Collections.Generic;

namespace FlightRoutesSystem.Application.Abstracts
{
    public class BaseService<T> where T : Entity
    {
        private BaseRepository<T> _repository;

        public BaseService(BaseRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual T Add(T entity)
        {
            return _repository.Add(entity);
        }

        public virtual void Remove(T entity)
        {
            _repository.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            _repository.Update(entity);
        }

        public virtual T GetById(long entity)
        {
            return _repository.GetById(entity);
        }

        public virtual List<T> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
