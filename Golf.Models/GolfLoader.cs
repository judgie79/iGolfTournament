using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Golf.Tournament.Controllers
{
    public class GolfLoader
    {

        public GolfLoader(string apiUrl)
        {
            this.ApiUrl = apiUrl;
        }

        public string ApiUrl { get; private set; }

        public TModel Load<TModel>(string requestUri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(requestUri).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsAsync<TModel>().Result;

                return dataObjects;
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public TModel Post<TModel>(string requestUri, TModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.PostAsJsonAsync(requestUri, model).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsAsync<TModel>().Result;

                return dataObjects;
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public TResultModel Post<TModel, TResultModel>(string requestUri, TModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.PostAsJsonAsync(requestUri, model).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsAsync<TResultModel>().Result;

                return dataObjects;
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public TResultModel Put<TModel, TResultModel>(string requestUri, TModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.PutAsJsonAsync(requestUri, model).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsAsync<TResultModel>().Result;

                return dataObjects;
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public TModel Put<TModel>(string requestUri, TModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.PutAsJsonAsync(requestUri, model).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsAsync<TModel>().Result;

                return dataObjects;
            }
            else
            {
                throw new ApiException(response);
            }
        }

        public bool Delete<TModel>(string requestUri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.DeleteAsync(requestUri).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new ApiException(response);
            }
        }
    }
}