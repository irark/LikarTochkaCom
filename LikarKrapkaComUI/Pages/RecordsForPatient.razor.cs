using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using LikarKrapkaComUI.Services;
using LikarKrapkaComEntities.Models;
using Newtonsoft.Json;
using System.Net.Http;
using LikarKrapkaComUI.Services.HttpServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using LikarKrapkaComEntities.ViewModel;
using DevExpress.Blazor;
using Blazored.Toast.Services;

namespace LikarKrapkaComUI.Pages
{
    public partial class RecordsForPatient
    {
        [Inject] RecordsService RecordsService { get; set; }
        [Inject] private DoctorsService DoctorsService { get; set; }
        [Inject] private IToastService toastService { get; set; }
        [Parameter] public int patientId { get; set; }
        public int CurrentPatientId { get; set; }

        private List<Record> records { get; set; }
        private DxDataGrid<Record> grid;
        private RecordViewModel EditContext = null;
        async void Refresh()
        {
            var res = await RecordsService.GetRecordsByPatientId(patientId);

            if (res == null)
                records = new List<Record>();


            records = res;

        }
        protected override async Task OnInitializedAsync()
        {
            Refresh();
        }
        async Task HandleValidSubmit()
        {
            EditContext.DataItem.DoctorId = EditContext.DoctorId;
            EditContext.DataItem.PatientId = patientId;
            EditContext.DataItem.Date = EditContext.Date;
            EditContext.DataItem.Note = EditContext.Note;
            EditContext.DataItem.Id = EditContext.Id;
            if (EditContext.IsNewRow)
            {
                EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
                var result = await RecordsService.Insert(EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            else
            {
                var result = await RecordsService.Update(EditContext.DataItem.Id, EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            Refresh();
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
            await grid.CancelRowEdit();
            EditContext = null;
        }
        private async Task<IEnumerable<PersonList>> GetListOfDoctors(CancellationToken ct = default)
        {
            var res = await DoctorsService.GetAllDoctors();

            if (res == null)
                return new List<PersonList>();
            var patients = new List<PersonList>();
            foreach (var item in res)
            {
                patients.Add(new PersonList() { Id = item.Id, Name = item.FirstName + ' ' + item.LastName });
            }

            return patients;
        }
        void OnCustomDisabledDates(CalendarCustomDisabledDateEventArgs args)
        {
            args.IsDisabled = GetListOfDates().Result.Exists(d => DaysEqual(d, args.Date)) || args.Date.DayOfWeek == DayOfWeek.Sunday || args.Date.DayOfWeek == DayOfWeek.Saturday;

        }
        bool DaysEqual(DateTime date1, DateTime date2)
        {
            return (date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear && date1.Hour == date2.Hour && date1.Minute == date2.Minute && date1.Second == date2.Second);
        }
        private async Task<List<DateTime>> GetListOfDates()
        {
            var res = new List<DateTime>();
            var patientsDate = await RecordsService.GetDateForPatient(patientId);
            var doctorsDate = await RecordsService.GetDateForDoctor(EditContext.DoctorId);
            if (patientsDate == null)
            {
                patientsDate = new List<DateTime>();
            }
            if (doctorsDate == null)
            {
                doctorsDate = new List<DateTime>();
            }
            res.AddRange(patientsDate);
            res.AddRange(doctorsDate);
            return res;
        }
        void OnRowEditStarting(Record record)
        {
            EditContext = new RecordViewModel(record);
            EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
        }
        async Task OnCancelButtonClick()
        {
            await grid.CancelRowEdit();
            EditContext = null;
        }
        async Task OnRowRemoving(Record dataItem)
        {

            var result = await RecordsService.Remove(dataItem.Id);
            if (!result)
                toastService.ShowError("Неможливо видалити запис, оскільки в базі є пов'язані з нею дані");
            Refresh();
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
        }
        async Task OnRowUpdating(Record dataItem, IDictionary<string, object> newValue)
        {
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        dataItem.Id = (int)item.Value;
                        break;
                    case "DoctorId":
                        dataItem.DoctorId = (int)item.Value;
                        break;
                    case "PatientId":
                        dataItem.PatientId = (int)item.Value;
                        break;
                    case "Date":
                        dataItem.Date = (DateTime)item.Value;
                        break;
                    case "Note":
                        dataItem.Note = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await RecordsService.Update(dataItem.Id, dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        async Task OnRowInserting(IDictionary<string, object> newValue)
        {
            Record dataItem = new Record();
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        dataItem.Id = (int)item.Value;
                        break;
                    case "DoctorId":
                        dataItem.DoctorId = (int)item.Value;
                        break;
                    case "PatientId":
                        dataItem.PatientId = (int)item.Value;
                        break;
                    case "Date":
                        dataItem.Date = (DateTime)item.Value;
                        break;
                    case "Note":
                        dataItem.Note = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await RecordsService.Insert(dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        Task OnInitNewRow(Dictionary<string, object> values)
        {

            return Task.CompletedTask;
        }
    }
}
