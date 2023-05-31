using ESchedule.Models;
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

            //for (int i = 1; i <= 2; i++)
            //{
            //    modelBuilder.Entity<ClassViewModel>().HasData(new ClassViewModel
            //    {
            //        Id = i,
            //        Name = $"{i} клас",
            //        Description = $"{i} описання класу",
            //        IdUserAdmin = 3,
            //        CodeToJoin = $"TEST{i}"
            //    });
            //}

            //    modelBuilder.Entity<UserAccountViewModel>().HasData(new UserAccountViewModel
            //    {
            //        Id = 1,
            //        Name = "Сеня",
            //        SurName = "Норков",
            //        PatronymicName = "Сергеевич",
            //        Role = "Учень",
            //        Email = "email1@gmail.com",
            //        Password = "Password1",
            //    });

            //    modelBuilder.Entity<UserAccountViewModel>().HasData(new UserAccountViewModel
            //    {
            //        Id = 2,
            //        Name = "Петя",
            //        SurName = "Петьков",
            //        PatronymicName = "Петкевич",
            //        Role = "Учень",
            //        Email = "email2@gmail.com",
            //        Password = "Password2",
            //    });

            //    modelBuilder.Entity<UserAccountViewModel>().HasData(new UserAccountViewModel
            //    {
            //        Id = 3,
            //        Name = "Елена",
            //        SurName = "Коваленка",
            //        PatronymicName = "Александровна",
            //        Role = "Вчитель",
            //        Email = "email3@gmail.com",
            //        Password = "Password3",
            //    });

            //    for (int i = 1; i <= 5; i++)
            //    {
            //        modelBuilder.Entity<LessonViewModel>().HasData(new LessonViewModel
            //        {
            //            Id = i,
            //            NameLesson = $"Тестова Назва{i}",
            //            DescriptionLesson = $"Тестова Описання{i}",
            //            Created = DateTime.Now,
            //            BeginTime = new(2030, 9, i, 8, 0, 0), //год, месяц, день, час, минуту и секунду.
            //            EndTime = new(2030, 9, i, 8, 45, 0),
            //        });
            //    }
        }

        public DbSet<ClassViewModel> Classes { get; set; } 
        public DbSet<LessonViewModel> Lessons { get; set; }
        public DbSet<UserAccountViewModel> Users { get; set; }
    }
}
