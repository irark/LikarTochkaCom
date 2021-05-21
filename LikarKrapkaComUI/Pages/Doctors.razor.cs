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
    public partial class Doctors
    {
        [Inject] private DoctorsService DoctorsService { get; set; }
        [Inject] private SpecializationsService SpecializationsService { get; set; }
        [Inject] private OfficesService OfficesService { get; set; }
        [Inject] private HospitalsService HospitalsService { get; set; }
        [Inject] private IToastService toastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Parameter] public int hospitalId { get; set; }
        [Parameter] public int specializationId { get; set; }
        [Parameter] public int officeId { get; set; }
        private List<Doctor> doctors { get; set; }
        private DoctorViewModel EditContext = null;
        private DxDataGrid<Doctor> grid;
        async void Refresh()
        {
            if (hospitalId != 0)
            {
                doctors = await DoctorsService.GetDoctorsByHospitalId(hospitalId);

            }
            else if (specializationId != 0)
            {
                doctors = await DoctorsService.GetDoctorsBySpecializationId(specializationId);

            }
            else if (officeId != 0)
            {
                doctors = await DoctorsService.GetDoctorsByOfficeId(officeId);

            }

            if (doctors == null)
                doctors = new List<Doctor>();
        }
        protected override async Task OnInitializedAsync()
        {
            Refresh();
        }
        private async Task<IEnumerable<Specialization>> GetListOfSpecializations(CancellationToken ct = default)
        {
            var res = await SpecializationsService.GetAllSpecializations();

            if (res == null)
                return new List<Specialization>();


            return res;
        }
        private async Task<IEnumerable<Office>> GetListOfOffices(CancellationToken ct = default)
        {
            var res = await OfficesService.GetAllOffices();

            if (res == null)
                return new List<Office>();


            return res;
        }
        private async Task<IEnumerable<Hospital>> GetListOfHospitals(CancellationToken ct = default)
        {
            var res = await HospitalsService.GetAllHospitals();

            if (res == null)
                return new List<Hospital>();


            return res;
        }
        async Task HandleValidSubmit()
        {
            EditContext.DataItem.FirstName = EditContext.FirstName;
            EditContext.DataItem.LastName = EditContext.LastName;
            EditContext.DataItem.PhoneNumber = EditContext.PhoneNumber;
            EditContext.DataItem.OfficeId = EditContext.OfficeId;
            EditContext.DataItem.HospitalId = EditContext.HospitalId;
            EditContext.DataItem.SpecializationId = EditContext.SpecializationId;
            EditContext.DataItem.Id = EditContext.Id;

            if (EditContext.IsNewRow)
            {
                EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
                var result = await DoctorsService.Insert(EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            else
            {
                var result = await DoctorsService.Update(EditContext.DataItem.Id, EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            Refresh();
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
            await grid.CancelRowEdit();
            EditContext = null;
        }
        void OnRowEditStarting(Doctor doctor)
        {
            EditContext = new DoctorViewModel(doctor);
            EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
        }
        async Task OnCancelButtonClick()
        {
            await grid.CancelRowEdit();
            EditContext = null;
        }
        async Task OnRowRemoving(Doctor dataItem)
        {
            var result = await DoctorsService.Remove(dataItem.Id);
            if(!result)
                toastService.ShowError("Неможливо видалити лікаря, оскільки в базі є пов'язані з нею дані");
            Refresh();
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
        }
        async Task OnRowUpdating(Doctor dataItem, IDictionary<string, object> newValue)
        {
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        dataItem.Id = (int)item.Value;
                        break;
                    case "FirstName":
                        dataItem.FirstName = (string)item.Value;
                        break;
                    case "LastName":
                        dataItem.LastName = (string)item.Value;
                        break;
                    case "PhoneNumber":
                        dataItem.PhoneNumber = (string)item.Value;
                        break;
                    case "OfficeId":
                        dataItem.OfficeId = (int)item.Value;
                        break;
                    case "SpecializationId":
                        dataItem.SpecializationId = (int)item.Value;
                        break;
                    case "HospitalId":
                        dataItem.HospitalId = (int)item.Value;
                        break;
                    default:
                        break;
                }
            }

            var result = await DoctorsService.Update(dataItem.Id, dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        async Task OnRowInserting(IDictionary<string, object> newValue)
        {
            Doctor dataItem = new Doctor();
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        dataItem.Id = (int)item.Value;
                        break;
                    case "FirstName":
                        dataItem.FirstName = (string)item.Value;
                        break;
                    case "LastName":
                        dataItem.LastName = (string)item.Value;
                        break;
                    case "PhoneNumber":
                        dataItem.PhoneNumber = (string)item.Value;
                        break;
                    case "OfficeId":
                        dataItem.OfficeId = (int)item.Value;
                        break;
                    case "SpecializationId":
                        dataItem.SpecializationId = (int)item.Value;
                        break;
                    case "HospitalId":
                        dataItem.HospitalId = (int)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await DoctorsService.Insert(dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        Task OnInitNewRow(Dictionary<string, object> values)
        {

            return Task.CompletedTask;
        }
        private async Task GoToDoctorInfo(Doctor doctor)
        {
            NavigationManager.NavigateTo($"/recordsForDoctor/{doctor.Id}");
        }
    }
}
