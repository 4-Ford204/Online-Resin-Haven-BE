using Ardalis.Result;
using Ardalis.SharedKernel;
using OPS.Domain.Constants.Enums;
using OPS.UseCases.Interfaces.InternalServices.Pets;

namespace OPS.UseCases.Requests.Pets.Queries
{
    public record SearchPetsQuery(SearchPetsRequest Request) : IQuery<Result<List<SearchPetsResponse>>>;

    public class SearchPetsHandler(ISeachPets service) : IQueryHandler<SearchPetsQuery, Result<List<SearchPetsResponse>>>
    {
        public async Task<Result<List<SearchPetsResponse>>> Handle(SearchPetsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pets = await service.Execute(request.Request);
                return Result.Success(pets);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }

    public class SearchPetsRequest { }

    public class SearchPetsResponse
    {
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public required Gender Gender { get; set; }
        public required int Price { get; set; }
        public required string Image { get; set; }
        public string? Description { get; set; }
    }
}
