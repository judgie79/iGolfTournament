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
}