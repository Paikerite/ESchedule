using Azure;
using ESchedule.Models;
using ESchedule.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace ESchedule.Services
{
    public class ClassService : IClassService
    {
        private readonly HttpClient httpClient;
        public ClassService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ClassViewModel> AddUserToClassByCode(JoinClassModel joinClass)
        {
            var JsonRequest = JsonConvert.SerializeObject(joinClass);
            var content = new StringContent(JsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await httpClient.PatchAsync("api/ClassWebAPI/AddUserToClassByCode", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ClassViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return default(ClassViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<ClassViewModel> DeleteClass(int id)
        {
            var response = await httpClient.DeleteAsync($"api/ClassWebAPI/{id}");
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<ClassViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(ClassViewModel);
            }
            return default(ClassViewModel);
        }

        public async Task<ClassViewModel> GetClass(int id)
        {
            var response = await this.httpClient.GetAsync($"api/ClassWebAPI/{id}");
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<ClassViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(ClassViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<ClassViewModel> GetClassByCodeJoin(string codeToJoin)
        {
            var response = await this.httpClient.GetAsync($"api/ClassWebAPI/GetClassByJoinCode/{codeToJoin}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ClassViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(ClassViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<IEnumerable<ClassViewModel>> GetClassesByUserName(string userName)
        {
            var response = await this.httpClient.GetAsync($"api/ClassWebAPI/GetClassesByUserName/{userName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ClassViewModel>>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Enumerable.Empty<ClassViewModel>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<IEnumerable<ClassViewModel>> GetClassesBySearchName(string nameClass, string nameUser)
        {
            var response = await this.httpClient.GetAsync($"api/ClassWebAPI/GetClassesByNameSearch/{nameUser}/{nameClass}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ClassViewModel>>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Enumerable.Empty<ClassViewModel>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<IEnumerable<ClassViewModel>> GetClassesByAdminId(int id)
        {
            var response = await this.httpClient.GetAsync($"api/ClassWebAPI/GetClassesByAdminId/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ClassViewModel>>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Enumerable.Empty<ClassViewModel>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<ClassViewModel> PostClass(ClassViewModel classViewModel)
        {
            var response = await httpClient.PostAsJsonAsync<ClassViewModel>("api/ClassWebAPI", classViewModel);
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<ClassViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(ClassViewModel);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }

        public async Task<ClassViewModel> UpdateClass(int id, ClassViewModel classViewModel)
        {
            var JsonRequest = JsonConvert.SerializeObject(classViewModel);
            var content = new StringContent(JsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await httpClient.PutAsync($"api/ClassWebAPI/{id}", content);
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<ClassViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return classViewModel;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"http status:{response.StatusCode}, message:{message}");
            }
        }
    }
}
