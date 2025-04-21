using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Entities; // Assuming your Customer entity is here

namespace DataAccess.Repositories.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer, string> 
    {
        Task<Customer> GetCustomerWithOrdersAsync(string customerId);
    }
}