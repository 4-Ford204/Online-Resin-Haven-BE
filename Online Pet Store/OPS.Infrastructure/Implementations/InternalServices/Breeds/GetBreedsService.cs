using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Constants.Enums;
using OPS.Infrastructure.MSSQL;
using OPS.UseCases.Interfaces.InternalServices.Breeds;
using OPS.UseCases.Requests.Breeds.Queries;

namespace OPS.Infrastructure.Implementations.InternalServices.Breeds
{
    [Service(ServiceLifetime.Scoped)]
    public class GetBreedsService : IGetBreeds
    {
        private readonly DataContext dbContext;

        public GetBreedsService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GetBreedsResponse>> Execute()
        {
            var breeds = await dbContext.Breeds
                .Where(b => b.Status == Status.Active)
                .Include(b => b.Species)
                .Include(s => s.Pets)
                .ToListAsync();
            var result = breeds
                .Select(b => new GetBreedsResponse
                {
                    Name = b.Name,
                    Species = b.Species?.Name,
                    Pets = b.Pets.Select(p => p.Image)
                })
                .ToList();

            return result;
        }
    }
}
