using ESchedule.Data;
using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESchedule.Services
{
    public class UserService : IUserService
    { 
        private readonly EScheduleDbContext eScheduleDbContext;

        public UserService(EScheduleDbContext eScheduleDbContext)
        {
            this.eScheduleDbContext = eScheduleDbContext;
        }

        public async Task<UserAccountViewModel> GetUserById(int id)
        {
            //if (eScheduleDbContext.Users == null)
            //{
            //    return null;
            //}

            //var userAccountViewModel = await eScheduleDbContext.Users
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (userAccountViewModel == null)
            //{
            //    return default(UserAccountViewModel);
            //}

            //return userAccountViewModel;
            return null;
        }

        public async Task<UserAccountViewModel> GetUserByEmail(string email)
        {
            //if (eScheduleDbContext.Users == null)
            //{
            //    return null;
            //}

            //var userAccountViewModel = await eScheduleDbContext.Users
            //    .FirstOrDefaultAsync(m => m.Email == email);
            //if (userAccountViewModel == null)
            //{
            //    return default(UserAccountViewModel);
            //}

            //return userAccountViewModel;
            return null;
        }
    }
}
