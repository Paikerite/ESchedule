using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ESchedule.Services
{
    public class LessonService : ILessonService
    {
        private readonly HttpClient httpClient;

        public LessonService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<LessonViewModel> AddLesson(LessonViewModel model)
        {
            var response = await httpClient.PostAsJsonAsync<LessonViewModel>("api/LessonWebAPI", model);
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<LessonViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(LessonViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }

        }

        public async Task<LessonViewModel> DeleteLesson(int id)
        {

            var response = await httpClient.DeleteAsync($"api/LessonWebAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LessonViewModel>();
            }
            return default(LessonViewModel);

        }

        public async Task<LessonViewModel> GetLesson(int id)
        {

            var response = await this.httpClient.GetAsync($"api/LessonWebAPI/{id}");
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<LessonViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(LessonViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }

        }

        public async Task<LessonViewModel> AddClassesToLesson(AddClassesToLessonModel classesToLessonModel)
        {
            var JsonRequest = JsonConvert.SerializeObject(classesToLessonModel);
            var content = new StringContent(JsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await httpClient.PatchAsync("api/LessonWebAPI/AddClassesToLesson", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LessonViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return default(LessonViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<IEnumerable<LessonViewModel>> GetLessons()
        {

            var products = await this.httpClient.GetAsync("api/LessonWebAPI");
            if (products.IsSuccessStatusCode)
            {

                return await products.Content.ReadFromJsonAsync<IEnumerable<LessonViewModel>>();
            }
            else if (products.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Enumerable.Empty<LessonViewModel>();
            }
            else
            {
                var message = await products.Content.ReadAsStringAsync();
                throw new Exception($"http status:{products.StatusCode}, message:{message}");
            }

        }

        public async Task<IEnumerable<LessonViewModel>> GetLessonsByDateAndName(DateTime Date, string UserName)
        {

            var lessons = await this.httpClient.GetAsync($"api/LessonWebAPI/GetLessonsByDateAndName/{Date.ToString("u")}/{UserName}");
            if (lessons.IsSuccessStatusCode)
            {

                return await lessons.Content.ReadFromJsonAsync<IEnumerable<LessonViewModel>>();
            }
            else if (lessons.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Enumerable.Empty<LessonViewModel>();
            }
            else
            {
                var message = await lessons.Content.ReadAsStringAsync();
                throw new Exception($"http status:{lessons.StatusCode}, message:{message}");
            }

        }

        public async Task<LessonViewModel> UpdateLesson(int id, LessonViewModel model)
        {
            var JsonRequest = JsonConvert.SerializeObject(model);
            var content = new StringContent(JsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await httpClient.PutAsync($"api/LessonWebAPI/{id}", content);
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<LessonViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return default(LessonViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }
    }
}
