using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using LikarKrapkaComEntities.Models;
using Microsoft.Extensions.Configuration;


namespace LikarKrapkaComUI.Services.HttpServices
{
    public abstract class ServiceBase
    {
        protected readonly HttpClient _client;

        protected abstract string _apiControllerName { get; set; }
        protected ServiceBase()
        {
           
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44392");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.Timeout = TimeSpan.FromMinutes(5);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected string Url()
        {
            return $"/api/{_apiControllerName}";
        }

        protected string Url(string action)
        {
            return $"/api/{_apiControllerName}/{action}";
        }
        protected async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

           
            string s = response.StatusCode.ToString() + content;
            throw new Exception(s);
        }




    }
}
