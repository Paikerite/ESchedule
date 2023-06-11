using ESchedule.Models;

namespace ESchedule.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonViewModel>> GetLessons();
        Task<IEnumerable<LessonViewModel>> GetLessonsByDate(DateTime date);
        Task<LessonViewModel> GetLesson(int id);
        Task<LessonViewModel> UpdateLesson(int id, LessonViewModel model);
        Task<LessonViewModel> AddLesson(LessonViewModel model);
        Task<LessonViewModel> DeleteLesson(int id);
    }
}
