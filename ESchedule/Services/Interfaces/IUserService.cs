using ESchedule.Models;

namespace ESchedule.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterModel> GetUserByEmail (string email);
        Task<RegisterModel> GetUserById (int id);
    }
}
