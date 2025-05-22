using AppTrainingBE.Models;
//using AppTrainingBETeacher.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTrainingBE.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                    : base(options) 
        {   

        }

        public DbSet<Person> Persons { get; set; }


        #region Uno a Uno
        public DbSet<User> Users => Set<User>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Uno a Uno
            //// Configurar relación 1:1
            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Profile)
            //    .WithOne(p => p.User)
            //    .HasForeignKey<UserProfile>(p => p.UserId);

            //// Restricciones adicionales opcionales
            //modelBuilder.Entity<User>()
            //    .Property(u => u.Username)
            //    .IsRequired()
            //    .HasMaxLength(100);
            #endregion
        }
    }
}
