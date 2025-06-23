using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Constants.Enums;
using OPS.Infrastructure.MSSQL;
using OPS.UseCases.Interfaces.InternalServices.Pets;
using OPS.UseCases.Requests.Pets.Queries;

namespace OPS.Infrastructure.Implementations.InternalServices.Pets
{
    [Service(ServiceLifetime.Scoped)]
    public class SearchPetsService : ISeachPets
    {
        private readonly DataContext dbContext;

        public SearchPetsService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<SearchPetsResponse>> Execute(SearchPetsRequest request)
        {
            var query = dbContext.Pets
                .Where(p => p.Status == Status.Active && p.OwnerId == null)
                .AsQueryable();
            var pets = await query.Include(p => p.Breed).ThenInclude(b => b!.Species).ToListAsync();
            var result = pets
                .Select(p => new SearchPetsResponse
                {
                    Species = p.Breed?.Species?.Name,
                    Breed = p.Breed?.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    Price = p.Price,
                    Image = p.Image,
                    Description = p.Description
                })
                .ToList();

            return result;
        }
    }
}
