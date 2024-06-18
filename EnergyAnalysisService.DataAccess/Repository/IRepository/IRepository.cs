using System.Linq.Expressions;

namespace EnergyAnalysisService.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);
    T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}