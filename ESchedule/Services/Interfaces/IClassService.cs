using ESchedule.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESchedule.Services.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<ClassViewModel>> GetClassesByUserName (string userName);
        Task<ClassViewModel> GetClassByCodeJoin(string codeToJoin);
        Task<IEnumerable<ClassViewModel>> GetClassesBySearchName(string nameClass, string nameUser);
        Task<IEnumerable<ClassViewModel>> GetClassesByAdminId(int id);
        Task<ClassViewModel> AddUserToClassByCode(JoinClassModel joinClass);
        Task<ClassViewModel> GetClass(int id);
        Task<ClassViewModel> UpdateClass(int id, ClassViewModel classViewModel);
        Task<ClassViewModel> PostClass(ClassViewModel classViewModel);
        Task<ClassViewModel> DeleteClass(int id);
    }
}
