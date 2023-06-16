using ESchedule.Models;

namespace ESchedule.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserAccountViewModel> GetUserByEmail (string email);
        Task<UserAccountViewModel> GetUserById (int id);
    }
}
