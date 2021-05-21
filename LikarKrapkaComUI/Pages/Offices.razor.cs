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
    public partial class Offices
    {
        [Inject] private OfficesService OfficesService { get; set; }

        [Inject] private IToastService toastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private OfficeViewModel EditContext = null;
        private List<Office> offices { get; set; }
        private DxDataGrid<Office> grid;
        protected override async Task OnInitializedAsync()
        {
            var res = await OfficesService.GetAllOffices();

            if (res == null)
                offices = new List<Office>();


            offices = res;
        }
        async Task HandleValidSubmit()
        {
            EditContext.DataItem.Name = EditContext.Name;
            EditContext.DataItem.Id = EditContext.Id;
            if (EditContext.IsNewRow)
            {
                EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
                var result = await OfficesService.Insert(EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            else
            {
                var result = await OfficesService.Update(EditContext.DataItem.Id, EditContext.DataItem);
                if (!result)
                    toastService.ShowError("Неправильно заповнені дані");
            }
            var res = await OfficesService.GetAllOffices();

            if (res == null)
                offices = new List<Office>();


            offices = res;
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
            await grid.CancelRowEdit();
            EditContext = null;
        }
        void OnRowEditStarting(Office office)
        {
            EditContext = new OfficeViewModel(office);
            EditContext.StateHasChanged += () => { InvokeAsync(StateHasChanged); };
        }
        async Task OnCancelButtonClick()
        {
            await grid.CancelRowEdit();
            EditContext = null;
        }
        async Task OnRowRemoving(Office dataItem)
        {

            var result = await OfficesService.Remove(dataItem.Id);
            if (!result)
                toastService.ShowError("Неможливо видалити спеціалізацію, оскільки в базі є пов'язані з нею дані");
            var res = await OfficesService.GetAllOffices();

            if (res == null)
                offices = new List<Office>();


            offices = res;
            await InvokeAsync(StateHasChanged);
            await grid.Refresh();
        }
        async Task OnRowUpdating(Office dataItem, IDictionary<string, object> newValue)
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
            var result = await OfficesService.Update(dataItem.Id, dataItem);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        async Task OnRowInserting(IDictionary<string, object> newValue)
        {
            Office newOffice = new Office();
            foreach (var item in newValue)
            {
                switch (item.Key)
                {
                    case "Id":
                        newOffice.Id = (int)item.Value;
                        break;
                    case "Name":
                        newOffice.Name = (string)item.Value;
                        break;
                    default:
                        break;
                }
            }
            var result = await OfficesService.Insert(newOffice);
            if (!result)
                toastService.ShowError("Неправильно заповнені дані");
        }
        Task OnInitNewRow(Dictionary<string, object> values)
        {

            return Task.CompletedTask;
        }
        private async Task GoToDoctors(Office office)
        {
            NavigationManager.NavigateTo($"/doctors/0/0/{office.Id}");
        }
    }
}
