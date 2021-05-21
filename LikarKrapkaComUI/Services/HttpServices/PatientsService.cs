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
    public class PatientsService : ServiceBase
    {
        protected override string _apiControllerName { get; set; }
        public PatientsService() : base()
        {
            _apiControllerName = "Patients";
        }
        public async Task<List<Patient>> GetAllPatients()
        {
            var apiUrl = Url("GetPatients");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Patient>>(result);
        }
        public async Task<bool> Remove(int patientId)
        {
            var apiUrl = Url($"DeletePatient/{patientId}");
            var response = _client.DeleteAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Update(int patientId, Patient newPatient)
        {
            var apiUrl = Url($"UpdatePatient");
            var newItem = new StringContent(JsonConvert.SerializeObject(newPatient), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(apiUrl, newItem).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Insert(Patient newPatient)
        {
            var apiUrl = Url("InsertPatient");
            var response = _client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(newPatient), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
