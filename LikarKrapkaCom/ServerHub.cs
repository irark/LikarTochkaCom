using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LikarKrapkaComAPI
{
    public class ServerHub : Hub
    {
        private static IHubCallerClients ClientsProxy = null;

        public ServerHub()
        { }

        public override Task OnConnectedAsync()
        {
            try
            {
                ClientsProxy = Clients;

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerHub OnConnectedAsync Exception");
                Console.WriteLine(ex);
                return Task.CompletedTask;
            }
        }

        public void OnSessionOrReceiptChanged()
        {
            _ = ClientsProxy?.All.SendAsync("OnSessionOrReceiptChanged");
        }

        //{
        //    public override async Task OnConnectedAsync()
        //    {
        //        await Clients.Caller.SendAsync("Notify", "client was connected");
        //        await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        //        await base.OnConnectedAsync();
        //    }

        //    public override async Task OnDisconnectedAsync(Exception exception)
        //    {
        //        await Clients.Caller.SendAsync("Notify", "client was disconnected");
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
        //        await base.OnDisconnectedAsync(exception);
        //    }


        //    public async Task Send(string message)
        //    {
        //        await Clients.Caller.SendAsync("Notify", message + " was added");
        //    }
    }
}
