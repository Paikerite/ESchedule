using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

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
            try
            {
                var response = await httpClient.PostAsJsonAsync<LessonViewModel>("api/LessonWebAPI", model);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(LessonViewModel);
                    }

                    return await response.Content.ReadFromJsonAsync<LessonViewModel>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{response.StatusCode}, message:{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LessonViewModel> DeleteLesson(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/LessonWebAPI/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LessonViewModel>();
                }
                return default(LessonViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LessonViewModel> GetLesson(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/LessonWebAPI/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(LessonViewModel);
                    }

                    return await response.Content.ReadFromJsonAsync<LessonViewModel>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{response.StatusCode}, message:{message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LessonViewModel>> GetLessons()
        {
            try
            {
                var products = await this.httpClient.GetAsync("api/LessonWebAPI");
                if (products.IsSuccessStatusCode)
                {
                    if (products.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<LessonViewModel>();
                    }

                    return await products.Content.ReadFromJsonAsync<IEnumerable<LessonViewModel>>();
                }
                else
                {
                    var message = await products.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{products.StatusCode}, message:{message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LessonViewModel>> GetLessonsByDate(DateTime date)
        {
            try
            {
                var lessons = await this.httpClient.GetAsync($"api/LessonWebAPI/{date}");
                if (lessons.IsSuccessStatusCode)
                {
                    if (lessons.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<LessonViewModel>();
                    }

                    return await lessons.Content.ReadFromJsonAsync<IEnumerable<LessonViewModel>>();
                }
                else
                {
                    var message = await lessons.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{lessons.StatusCode}, message:{message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LessonViewModel> UpdateLesson(int id, LessonViewModel model)
        {
            try
            {
                var JsonRequest = JsonConvert.SerializeObject(model);
                var content = new StringContent(JsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await httpClient.PutAsync($"api/LessonWebAPI/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LessonViewModel>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{response.StatusCode}, message:{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
