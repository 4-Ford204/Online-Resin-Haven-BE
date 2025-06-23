using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Constants.Enums;
using OPS.Infrastructure.MSSQL;
using OPS.UseCases.Interfaces.InternalServices.Customers;
using OPS.UseCases.Requests.Customers.Queries;

namespace OPS.Infrastructure.Implementations.InternalServices.Customers
{
    [Service(ServiceLifetime.Scoped)]
    public class SearchCustomersService : ISearchCustomers
    {
        private readonly DataContext dbContext;

        public SearchCustomersService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<SearchCustomersResponse>> Execute(SearchCustomersRequest request)
        {
            var query = dbContext.Customers.Where(c => c.Status == Status.Active).AsQueryable();

            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    query = query.Where(c => string.Concat(c.FirstName, c.LastName).ToLower().Contains(request.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query = query.Where(c => c.Email.Contains(request.Email));
                }

                if (!string.IsNullOrEmpty(request.Phone))
                {
                    query = query.Where(c => c.Phone != null && c.Phone.Contains(request.Phone));
                }

                if (!string.IsNullOrEmpty(request.Address))
                {
                    query = query.Where(c => c.Address != null && c.Address.Contains(request.Address));
                }
            }

            var customers = await query.ToListAsync();
            var result = customers
                .Select(c => new SearchCustomersResponse
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Gender = c.Gender,
                    Phone = c.Phone,
                    Address = c.Address
                })
                .ToList();

            return result;
        }
    }
}
