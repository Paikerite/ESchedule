using ESchedule.Models;

namespace ESchedule.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonViewModel>> GetLessons();
        Task<IEnumerable<LessonViewModel>> GetLessonsByDateAndName(DateTime Date, string UserName);
        Task<LessonViewModel> AddClassesToLesson(AddClassesToLessonModel classesToLessonModel);
        Task<LessonViewModel> GetLesson(int id);
        Task<LessonViewModel> UpdateLesson(int id, LessonViewModel model);
        Task<LessonViewModel> AddLesson(LessonViewModel model);
        Task<LessonViewModel> DeleteLesson(int id);
    }
}
