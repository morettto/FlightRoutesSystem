using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FlightRoutesSystem.DataAccess.Abstracts
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected FlightRoutesSystemContext _context;

        public BaseRepository(FlightRoutesSystemContext context)
        {
            _context = context;
        }

        public virtual T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public virtual T GetById(long entityId)
        {
            return _context.Set<T>().Find(entityId);
        }

        public virtual List<T> GetAll()
        {
            List<T> entityList = new List<T>();

            var entitiesFound = _context.Set<T>().AsNoTracking();

            foreach (var item in entitiesFound)
            {
                entityList.Add(item);
            }
            return entityList;
        }
    }
}
