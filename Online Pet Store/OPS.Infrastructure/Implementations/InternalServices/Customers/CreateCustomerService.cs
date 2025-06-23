using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Entities;
using OPS.Infrastructure.MSSQL;
using OPS.UseCases.Interfaces.InternalServices.Customers;
using OPS.UseCases.Requests.Customers.Commands;

namespace OPS.Infrastructure.Implementations.InternalServices.Customers
{
    [Service(ServiceLifetime.Scoped)]
    public class CreateCustomerService : ICreateCustomer
    {
        private readonly DataContext dbContext;

        public CreateCustomerService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await dbContext.Customers.AnyAsync(c => c.Email.Equals(email));
        }

        public async Task<CreateCustomerResponse> Execute(CreateCustomerRequest request)
        {
            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Gender = request.Gender,
                Phone = request.Phone,
                Address = request.Address
            };

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();

            return new CreateCustomerResponse
            {
                Name = $"{customer.FirstName} {customer.LastName}"
            };
        }
    }
}
