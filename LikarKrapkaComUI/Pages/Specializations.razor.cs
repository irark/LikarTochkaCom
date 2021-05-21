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
    public partial class Specializations
    {
        [Inject] private SpecializationsService SpecializationsService { get; set; }

        [Inject] private IToastService toastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private SpecializationViewModel EditContext = null;
        private List<Specialization> specializations { get; set; }
        private DxDataGrid<Specialization> grid;
        async void Refresh()
        {
            var res = await SpecializationsService.GetAllSpecializations();

            if (res == null)
                specializations = new List<Specialization>();


            specializations = res;
        }
        protected override async Task OnInitializedAsync()
        {
            Refresh();
        }
        async Task HandleValidSubmit()
        {
            EditContext.DataItem.Name = EditContext.Name;
            EditContext.DataItem.Id = EditContext.Id;
            if (EditContext.IsNewRow)
            {
                EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
                var result = await SpecializationsService.Insert(EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            else
            {
                var result = await SpecializationsService.Update(EditContext.DataItem.Id, EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            Refresh();
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
            await grid.CancelRowEdit();
            EditContext = null;
        }
        void OnRowEditStarting(Specialization specialization)
        {
            EditContext = new SpecializationViewModel(specialization);
            EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
        }
        async Task OnCancelButtonClick()
        {
            await grid.CancelRowEdit();
            EditContext = null;
        }
        async Task OnRowRemoving(Specialization dataItem)
        {

            var result = await SpecializationsService.Remove(dataItem.Id);
            if (!result)
                toastService.ShowError("Неможливо видалити спеціалізацію, оскільки в базі є пов'язані з нею дані");
            Refresh();
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
        }
        async Task OnRowUpdating(Specialization dataItem, IDictionary<string, object> newValue)
        {
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        dataItem.Id = (int)item.Value;
                        break;
                    case "Name":
                        dataItem.Name = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await SpecializationsService.Update(dataItem.Id, dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        async Task OnRowInserting(IDictionary<string, object> newValue)
        {
            Specialization newSpecialization = new Specialization();
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        newSpecialization.Id = (int)item.Value;
                        break;
                    case "Name":
                        newSpecialization.Name = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await SpecializationsService.Insert(newSpecialization);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        Task OnInitNewRow(Dictionary<string, object> values)
        {

            return Task.CompletedTask;
        }
        private async Task GoToDoctors(Specialization specialization)
        {
            NavigationManager.NavigateTo($"/doctors/0/{specialization.Id}/0");
        }

    }
}
