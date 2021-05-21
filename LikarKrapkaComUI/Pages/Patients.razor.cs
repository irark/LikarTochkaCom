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
    public partial class Patients
    {
        [Inject] private PatientsService PatientsService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private List<Patient> patients { get; set; }
        private PatientViewModel EditContext = null;
        private DxDataGrid<Patient> grid;
        [Inject] private IToastService toastService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var res = await PatientsService.GetAllPatients();

            if (res == null)
                patients = new List<Patient>();


            patients = res;
        }
        
        async Task HandleValidSubmit()
        {
            EditContext.DataItem.FirstName = EditContext.FirstName;
            EditContext.DataItem.LastName = EditContext.LastName;
            EditContext.DataItem.PhoneNumber = EditContext.PhoneNumber;
            EditContext.DataItem.Id = EditContext.Id;

            if (EditContext.IsNewRow)
            {
                EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
                var result = await PatientsService.Insert(EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            else
            {
                var result = await PatientsService.Update(EditContext.DataItem.Id, EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            var res = await PatientsService.GetAllPatients();

            if (res == null)
                patients = new List<Patient>();


            patients = res;
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
            await grid.CancelRowEdit();
            EditContext = null;
        }
        void OnRowEditStarting(Patient patient)
        {
            EditContext = new PatientViewModel(patient);
            EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
        }
        async Task OnCancelButtonClick()
        {
            await grid.CancelRowEdit();
            EditContext = null;
        }
        async Task OnRowRemoving(Patient dataItem)
        {
            var result = await PatientsService.Remove(dataItem.Id);
            if (!result)
                toastService.ShowError("Неможливо видалити лікаря, оскільки в базі є пов'язані з нею дані");
            var res = await PatientsService.GetAllPatients();

            if (res == null)
                patients = new List<Patient>();


            patients = res;
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
        }
        async Task OnRowUpdating(Patient dataItem, IDictionary<string, object> newValue)
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
                    default:
                        break;
                }
            }
            var result = await PatientsService.Update(dataItem.Id, dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        async Task OnRowInserting(IDictionary<string, object> newValue)
        {
            Patient dataItem = new Patient();
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
                    default:
                        break;
                }
            }
            var result = await PatientsService.Insert(dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        Task OnInitNewRow(Dictionary<string, object> values)
        {

            return Task.CompletedTask;
        }
        private async Task GoToPatientInfo(Patient patient)
        {
            NavigationManager.NavigateTo($"/recordsForPatient/{patient.Id}");
        }
    }
}
