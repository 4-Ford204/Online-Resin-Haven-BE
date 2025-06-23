using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPS.Domain.Entities;
using OPS.Infrastructure.MSSQL.Interceptors;

namespace OPS.Infrastructure.MSSQL
{
    public class DataContext : DbContext
    {
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Pet> Pets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Customer.API"))
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("MSSQL");

                optionsBuilder.UseSqlServer(connectionString);
            }

            optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}
