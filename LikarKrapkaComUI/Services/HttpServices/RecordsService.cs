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
    public class RecordsService : ServiceBase
    {
        protected override string _apiControllerName { get; set; }
        public RecordsService() : base()
        {
            _apiControllerName = "Records";
        }
        public async Task<List<Record>> GetAllRecords()
        {
            var apiUrl = Url("GetRecords");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Record>>(result);
        }
        public async Task<List<DateTime>> GetDateForDoctor(int doctorId)
        {
            var apiUrl = Url($"GetDateForDoctor/{doctorId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DateTime>>(result);
        }
        public async Task<List<DateTime>> GetDateForPatient(int patientId)
        {
            var apiUrl = Url($"GetDateForPatient/{patientId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DateTime>>(result);
        }
        public async Task<Record> GetRecord(int recordId)
        {
            var apiUrl = Url($"GetRecord/{recordId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Record>(result);
        }
        public async Task<List<Record>> GetRecordsByDoctorId(int doctorId)
        {
            var apiUrl = Url($"GetRecordsByDoctorId/{doctorId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Record>>(result);
        }
        public async Task<List<Record>> GetRecordsByPatientId(int patientId)
        {
            var apiUrl = Url($"GetRecordsByPatientId/{patientId}");
            var response = _client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Record>>(result);
        }
        public async Task<bool> Remove(int recordId)
        {
            var apiUrl = Url($"DeleteRecord/{recordId}");
            var response = _client.DeleteAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Update(int recordId, Record newRecord)
        {
            var apiUrl = Url($"UpdateRecord");
            var newItem = new StringContent(JsonConvert.SerializeObject(newRecord), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(apiUrl, newItem).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        public async Task<bool> Insert(Record newRecord)
        {
            var apiUrl = Url("InsertRecord");
            var response = _client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(newRecord), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
