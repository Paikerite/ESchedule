﻿using ESchedule.Models;
using Microsoft.EntityFrameworkCore;

namespace ESchedule.Data
{
    public class EScheduleDbContext : DbContext
    {
        public EScheduleDbContext(DbContextOptions<EScheduleDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ClassViewModel> Classes { get; set; } 
        public DbSet<LessonViewModel> Lessons { get; set; }
        public DbSet<UserAccountViewModel> Users { get; set; }
    }
}
