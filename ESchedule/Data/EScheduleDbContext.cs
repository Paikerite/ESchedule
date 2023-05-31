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

            for (int i = 1; i <= 2; i++)
            {
                modelBuilder.Entity<ClassViewModel>().HasData(new ClassViewModel
                {
                    Id = i,
                    Name = $"{i} класс"
                });
            }

            modelBuilder.Entity<UserViewModel>().HasData(new UserViewModel
            {
                Id = 1,
                Name = "Сеня",
                SurName = "Норков",
                PatronymicName = "Сергеевич",
                Role = "Учень",
                Email = "email1@gmail.com",
                Password = "Password1",
                IdClass = 1
            });

            modelBuilder.Entity<UserViewModel>().HasData(new UserViewModel
            {
                Id = 2,
                Name = "Петя",
                SurName = "Петьков",
                PatronymicName = "Петкевич",
                Role = "Учень",
                Email = "email2@gmail.com",
                Password = "Password2",
                IdClass = 2
            });

            modelBuilder.Entity<UserViewModel>().HasData(new UserViewModel
            {
                Id = 3,
                Name = "Елена",
                SurName = "Коваленка",
                PatronymicName = "Александровна",
                Role = "Вчитель",
                Email = "email3@gmail.com",
                Password = "Password3",
                IdClass = 1
            });

            for (int i = 1; i <= 5; i++)
            {
                modelBuilder.Entity<LessonViewModel>().HasData(new LessonViewModel
                {
                    Id = i,
                    NameLesson = $"Тестова Назва{i}",
                    DescriptionLesson = $"Тестова Описання{i}",
                    IdClass = 1,
                    IdUserTeacher = 3,
                    Created = DateTime.Now,
                    BeginTime = new(2030, 9, i, 8, 0, 0), //год, месяц, день, час, минуту и секунду.
                    EndTime = new(2030, 9, i, 8, 45, 0),
                });
            }
        }

        public DbSet<ClassViewModel> Classes { get; set; }
        public DbSet<LessonViewModel> Lessons { get; set; }
        public DbSet<UserViewModel> Users { get; set; }
    }
}
