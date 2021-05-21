using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikarKrapkaComUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;


namespace LikarKrapkaComUI.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase
    {
        [Inject]
        private HubClientService HubClientService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await HubClientService.ConnectAsync();
        }

    }
}
