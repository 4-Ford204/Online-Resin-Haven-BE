using Microsoft.Extensions.DependencyInjection;
using OPS.Infrastructure.MSSQL;
using OPS.UseCases.Interfaces.InternalServices.Pets;

namespace OPS.Infrastructure.Implementations.InternalServices.Pets
{
    [Service(ServiceLifetime.Scoped)]
    public class UpdatePetService : IUpdatePet
    {
        private readonly DataContext dbContext;

        public UpdatePetService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> UpdateOwnerId(int petId, int customerId)
        {
            var pet = dbContext.Pets.FirstOrDefault(p => p.Id == petId);

            if (pet == null)
            {
                return false;
            }

            pet.OwnerId = customerId;

            dbContext.Entry(pet).Property(p => p.OwnerId).IsModified = true;

            var result = await dbContext.SaveChangesAsync().ContinueWith(x => x.Result > 0);

            return result;
        }
    }
}
