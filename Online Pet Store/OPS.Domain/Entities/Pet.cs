using OPS.Domain.Constants.Enums;

namespace OPS.Domain.Entities
{
    public class Pet : BaseEntity
    {
        public int? BreedId { get; set; }
        public int? Age { get; set; }
        public required Gender Gender { get; set; }
        public required int Price { get; set; }
        public required string Image { get; set; }
        public string? Description { get; set; }
        public int? OwnerId { get; set; }

        public Breed? Breed { get; set; }
        public Customer? Owner { get; set; }
    }
}
