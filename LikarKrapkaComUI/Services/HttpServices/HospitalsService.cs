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
    public class HospitalsService: ServiceBase
    {
        protected override string _apiControllerName { get; set; }
        public HospitalsService() : base()
        {
            _apiControllerName = "Hospitals";
        }
        public async Task<List<Hospital>> GetAllHospitals()
        {
            var apiUrl = Url("GetHospitals");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Hospital>>(result);
        }
        public async Task<bool> Remove(int hospitalId)
        {
            var apiUrl = Url($"{hospitalId}");
            var response = _client.DeleteAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return false;
           
            return true;
        }
        public async Task<bool> Update(int hospitalId, Hospital newHospital)
        {
            var apiUrl = Url($"UpdateHospital");
            var newItem = new StringContent(JsonConvert.SerializeObject(newHospital), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(apiUrl, newItem).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Insert(Hospital newHospital)
        {
            var apiUrl = Url("InsertHospital");
            var response = _client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(newHospital), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
