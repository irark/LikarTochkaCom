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
    public class OfficesService: ServiceBase
    {
        protected override string _apiControllerName { get; set; }
        public OfficesService() : base()
        {
            _apiControllerName = "Offices";
        }
        public async Task<List<Office>> GetAllOffices()
        {
            var apiUrl = Url("GetOffices");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Office>>(result);
        }
        public async Task<bool> Remove(int officeId)
        {
            var apiUrl = Url($"DeleteOffice/{officeId}");
            var response = _client.DeleteAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Update(int officeId, Office newOffice)
        {
            var apiUrl = Url($"UpdateOffice/{officeId}");
            var newItem = new StringContent(JsonConvert.SerializeObject(newOffice), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(apiUrl, newItem).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Insert(Office newOffice)
        {
            var apiUrl = Url("InsertOffice");
            var response = _client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(newOffice), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
