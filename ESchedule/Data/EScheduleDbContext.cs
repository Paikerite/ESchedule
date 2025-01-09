using ESchedule.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESchedule.Data
{
    public class EScheduleDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public EScheduleDbContext(DbContextOptions<EScheduleDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ClassViewModel>()
            //    .HasMany(e => e.UsersAccount)
            //    .WithMany(e => e.Classes);
        }

        public DbSet<ClassViewModel> Classes { get; set; } 
        public DbSet<LessonViewModel> Lessons { get; set; }
        //public DbSet<UserAccountViewModel> Users { get; set; }
    }
}
