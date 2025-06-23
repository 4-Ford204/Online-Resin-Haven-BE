using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities;

namespace OPS.Infrastructure.MSSQL.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.BreedId).IsRequired(false);
            builder.Property(p => p.Age).IsRequired(false);
            builder.Property(p => p.Gender).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Image).IsRequired();
            builder.Property(p => p.Description).IsRequired(false);
            builder.Property(p => p.OwnerId).IsRequired(false);

            builder.HasOne(p => p.Breed)
                .WithMany(b => b.Pets)
                .HasForeignKey(p => p.BreedId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Owner)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
