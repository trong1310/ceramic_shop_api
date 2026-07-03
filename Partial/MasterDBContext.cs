
using Microsoft.EntityFrameworkCore;

namespace CeramicShopMasterApi.Databases
{
    public partial class MasterDBContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
               => optionsBuilder.LogTo(Console.WriteLine, LogLevel.Error);


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasMany(x => x.Images)
                    .WithOne()
                    .HasForeignKey(d => d.Owner)
                    .HasPrincipalKey(c => c.Slug);
            });
        }
    }
}
