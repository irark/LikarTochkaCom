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
    public partial class Hospitals
    {
        [Inject] private HospitalsService HospitalsService { get; set; }

        [Inject] private IToastService toastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private HospitalViewModel EditContext = null;
        private List<Hospital> hospitals { get; set; }
        private DxDataGrid<Hospital> grid;
        protected override async Task OnInitializedAsync()
        {
            var res = await HospitalsService.GetAllHospitals();

            if (res == null)
                hospitals = new List<Hospital>();


            hospitals = res;
        }
        async Task HandleValidSubmit()
        {
            EditContext.DataItem.Name = EditContext.Name;
            EditContext.DataItem.Address = EditContext.Address;
            EditContext.DataItem.Id = EditContext.Id;
            if (EditContext.IsNewRow)
            {
                EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
                var result = await HospitalsService.Insert(EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            else
            {
                var result = await HospitalsService.Update(EditContext.DataItem.Id, EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            var res = await HospitalsService.GetAllHospitals();

            if (res == null)
                hospitals = new List<Hospital>();


            hospitals = res;
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
            await grid.CancelRowEdit();
            EditContext = null;
        }
        void OnRowEditStarting(Hospital hospital)
        {
            EditContext = new HospitalViewModel(hospital);
            EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
        }
        async Task OnCancelButtonClick()
        {
            await grid.CancelRowEdit();
            EditContext = null;
        }
        async Task OnRowRemoving(Hospital dataItem)
        {
            
            var result = await HospitalsService.Remove(dataItem.Id);
            if(!result)
                toastService.ShowError("Неможливо видалити лікарню, оскільки в базі є пов'язані з нею дані");
            var res = await HospitalsService.GetAllHospitals();

            if (res == null)
                hospitals = new List<Hospital>();


            hospitals = res;
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
        }
        async Task OnRowUpdating(Hospital dataItem, IDictionary<string, object> newValue)
        {
            foreach(var item in newValue)
            {
                switch (item.Key){
                    case "Id" :
                        dataItem.Id = (int)item.Value;
                        break;
                    case "Name":
                        dataItem.Name = (string)item.Value;
                        break;
                    case "Address":
                        dataItem.Address = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await HospitalsService.Update(dataItem.Id, dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        async Task OnRowInserting(IDictionary<string, object> newValue)
        {
            Hospital newHospital = new Hospital();
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        newHospital.Id = (int)item.Value;
                        break;
                    case "Name":
                        newHospital.Name = (string)item.Value;
                        break;
                    case "Address":
                        newHospital.Address = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await HospitalsService.Insert(newHospital);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        Task OnInitNewRow(Dictionary<string, object> values)
        {
           
            return Task.CompletedTask;
        }
        private async Task GoToDoctors(Hospital hospital)
        {
            NavigationManager.NavigateTo($"/doctors/{hospital.Id}/0/0");
        }

    }
}
