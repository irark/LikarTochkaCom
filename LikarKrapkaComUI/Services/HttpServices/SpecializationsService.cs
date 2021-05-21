using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikarKrapkaComEntities.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using LikarKrapkaComEntities.ViewModel;

namespace LikarKrapkaComUI.Services.HttpServices
{
    public class SpecializationsService: ServiceBase
    {
        protected override string _apiControllerName { get; set; }
        public SpecializationsService() : base()
        {
            _apiControllerName = "Specializations";
        }
        public async Task<List<Specialization>> GetAllSpecializations()
        {
            var apiUrl = Url("GetSpecializations");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Specialization>>(result);
        }
        public async Task<bool> Remove(int specializationId)
        {
            var apiUrl = Url($"DeleteSpecialization/{specializationId}");
            var response = _client.DeleteAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Update(int specializationId, Specialization newSpecialization)
        {
            var apiUrl = Url($"UpdateSpecialization");
            var newItem = new StringContent(JsonConvert.SerializeObject(newSpecialization), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(apiUrl, newItem).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Insert(Specialization newSpecialization)
        {
            var apiUrl = Url("InsertSpecialization");
            var response = _client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(newSpecialization), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
