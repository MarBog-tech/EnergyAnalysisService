using System.Linq.Expressions;
using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EnergyAnalysisService.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T: class 
    {
        private readonly EnergyAnalysisServiceContext _db;
        public Repository(EnergyAnalysisServiceContext db)
        {
            _db = db;
        }

        public void Create(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked) {
                 query= _db.Set<T>();
            }
            else {
                 query = _db.Set<T>().AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties)) {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (filter != null) {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProp) => current.Include(includeProp.Trim()));
            }
            // if (!string.IsNullOrEmpty(includeProperties))
            // {
            //     foreach(var includeProp in includeProperties
            //                 .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //     {
            //         query = query.Include(includeProp);
            //     }
            // }
            return query.ToList();
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }
    }