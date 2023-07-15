using ESchedule.Data;
using ESchedule.Services;
using ESchedule.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ESchedule
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder.Services.AddDbContextPool<EScheduleDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"))
                /*o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))*/);

            //https://learn.microsoft.com/ru-ru/ef/core/querying/single-split-queries

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddHttpClient();
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7087/")
            });

            //builder.Services.AddTransient<IEmailSender, EmailSender>();
            //builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IClassService, ClassService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.Cookie.HttpOnly = true;
                    //option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    option.SlidingExpiration = true;
                    option.LoginPath = new PathString("/User/Login");
                    option.AccessDeniedPath = new PathString("/User/NotHaveRights");
                });

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            };

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Schedule}/{action=Index}/{id?}"); //{controller=Home}/{action=Index}/{id?}

            app.Run();
        }
    }
}