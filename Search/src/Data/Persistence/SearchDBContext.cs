using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using PetStore.Search.Domain;

namespace PetStore.Search.Data.Persistence
{
    public class SearchDBContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Animal>().ToCollection("animals");
        }
    }
}