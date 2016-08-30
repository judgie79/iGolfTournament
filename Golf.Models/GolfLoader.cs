using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Golf.Tournament.Controllers
{
    public class GolfLoader : IDisposable
    {
        private HttpClient client;

        public GolfLoader(string apiUrl)
        {
            this.ApiUrl = apiUrl;
            this.client = new HttpClient();
            InitConnection();
        }

        public string ApiUrl { get; private set; }
        
        private void InitConnection()
        {
            client.BaseAddress = new Uri(ApiUrl);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<TModel> LoadAsync<TModel>(string requestUri)
        {
                var response = await client.GetAsync(requestUri);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TModel>();
                }
                else
                {
                    throw new ApiException(response);
                }
        }

        public async Task<TModel> LoadAsync<TModel, TFormatter>(string requestUri)
            where TFormatter : MediaTypeFormatter, new()
        {
                var response = await client.GetAsync(requestUri);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TModel>(new[] { new TFormatter() });
                }
                else
                {
                    throw new ApiException(response);
                }
        }

        public async Task<TModel> PostAsync<TModel>(string requestUri, TModel model)
        {
            var response = await client.PostAsJsonAsync(requestUri, model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TModel>();
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public async Task<TResultModel> PostAsync<TModel, TResultModel>(string requestUri, TModel model)
        {
            var response = await client.PostAsJsonAsync(requestUri, model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TResultModel>();
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public async Task<TResultModel> PutAsync<TModel, TResultModel>(string requestUri, TModel model)
        {
            var response = await client.PutAsJsonAsync(requestUri, model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TResultModel>();
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public async Task<TModel> PutAsync<TModel>(string requestUri, TModel model)
        {
            var response = await client.PutAsJsonAsync(requestUri, model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TModel>();
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public async Task<bool> DeleteAsync<TModel>(string requestUri)
        {
            var response = await client.DeleteAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }

    public static class GolfLoaderExtensions
    {
        public static async Task<IEnumerable<Course>> SetHoles(this GolfLoader loader, IEnumerable<Course> courses)
        {
            List<Task<Course>> tasks = new List<Task<Course>>();

            foreach (var course in courses)
            {
                tasks.Add(SetHoles(loader, course));
            }
            var tasksArray = tasks.ToArray();

            return await Task.WhenAll(tasksArray);
        }

        public static async Task<Course> SetHoles(this GolfLoader loader, Course course)
        {
            var holesPerTeebox = await loader.GetHolesAsync(course.Id, course.TeeBoxes.Select(t => t.Id).ToArray());

            foreach (var teebox in course.TeeBoxes)
            {
                var holes = holesPerTeebox.Single(h => h.Key == teebox.Id).Value;
                var front = holes.Where(h => h.FrontOrBack == FrontOrBack.Front);
                var back = holes.Where(h => h.FrontOrBack == FrontOrBack.Back);

                teebox.Holes = new CourseHoles()
                {
                    Front = new CourseHoleCollection(front),
                    Back = new CourseHoleCollection(back)
                };
            }

            return course;
        }

        public static async Task<KeyValuePair<string, CourseHoleCollection>[]> GetHolesAsync(this GolfLoader loader, string courseId, string[] teeboxIds)
        {
            List<Task<KeyValuePair<string, CourseHoleCollection>>> tasks = new List<Task<KeyValuePair<string, CourseHoleCollection>>>();

            foreach (var teeboxId in teeboxIds)
            {
                tasks.Add(GetHolesAsync(loader, courseId, teeboxId));
            }
            var tasksArray = tasks.ToArray();

            return await Task.WhenAll(tasksArray);
        }

        public static async Task<KeyValuePair<string, CourseHoleCollection>> GetHolesAsync(this GolfLoader loader, string courseId, string teeboxId)
        {
            return new KeyValuePair<string, CourseHoleCollection>(teeboxId, await loader.LoadAsync<CourseHoleCollection>("courses/" + courseId + "/teeboxes/" + teeboxId + "/holes"));
        }
    }
}