namespace OPS.Domain.Entities
{
    public class Species : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<Breed> Breeds { get; set; } = [];
    }
}
