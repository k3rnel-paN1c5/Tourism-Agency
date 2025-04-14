using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataAccess.IRepositories
{
    public interface IRepository<T, TKey> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllByPredicateAsync(
            Expression<Func<T, bool>> predicate );
        public Task<T?> GetByIdAsync(TKey id);
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task SaveAsync();
    }
}