﻿using ESchedule.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESchedule.Services.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<ClassViewModel>> GetClassesByUserName (string userName);
        Task<ClassViewModel> GetClassByCodeJoin(string codeToJoin);
        Task<ClassViewModel> AddUserToClassByCode(JoinClassModel joinClass);
        Task<ClassViewModel> GetClass(int id);
        Task<ClassViewModel> UpdateClass(int id, ClassViewModel classViewModel);
        Task<ClassViewModel> PostClass(ClassViewModel classViewModel);
        Task<ClassViewModel> DeleteClass(int id);
    }
}