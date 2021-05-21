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
    public class DoctorsService: ServiceBase
    {
        protected override string _apiControllerName { get; set; }
        public DoctorsService() : base()
        {
            _apiControllerName = "Doctors";
        }
        public async Task<List<Doctor>> GetAllDoctors()
        {
            var apiUrl = Url("GetDoctors");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Doctor>>(result);
        }
        public async Task<Doctor> GetDoctor(int doctorId)
        {
            var apiUrl = Url($"GetDoctor/{doctorId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Doctor>(result);
        }
        public async Task<List<Doctor>> GetDoctorsByHospitalId(int hospitalId)
        {
            var apiUrl = Url($"GetDoctorsByHospitalId/{hospitalId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Doctor>>(result);
        }
        public async Task<List<Doctor>> GetDoctorsBySpecializationId(int specializationId)
        {
            var apiUrl = Url($"GetDoctorsBySpecializationId/{specializationId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Doctor>>(result);
        }
        public async Task<List<Doctor>> GetDoctorsByOfficeId(int officeId)
        {
            var apiUrl = Url($"GetDoctorsByOfficeId/{officeId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Doctor>>(result);
        }
        public async Task<bool> Remove(int doctorId)
        {
            var apiUrl = Url($"DeleteDoctor/{doctorId}");
            var response = _client.DeleteAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Update(int doctorId, Doctor newDoctor)
        {
            var apiUrl = Url($"UpdateDoctor");
            var newItem = new StringContent(JsonConvert.SerializeObject(newDoctor), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(apiUrl, newItem).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Insert(Doctor newDoctor)
        {
            var apiUrl = Url("InsertDoctor");
            var response = _client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(newDoctor), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
