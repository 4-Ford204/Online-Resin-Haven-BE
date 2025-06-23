namespace OPS.Domain.Entities
{
    public class Breed : BaseEntity
    {
        public required string Name { get; set; }
        public int? SpeciesId { get; set; }

        public Species? Species { get; set; }
        public ICollection<Pet> Pets { get; set; } = [];
    }
}
