using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataAccess.Repositories.IRepositories
{
    public interface IRepository<T, TKey> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllByPredicateAsync(Expression<Func<T, bool>> predicate);
        public Task<T?> GetByIdAsync(TKey id);
        public Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate);
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public void DeleteByIdAsync(TKey id);
        public Task SaveAsync();
    }
}