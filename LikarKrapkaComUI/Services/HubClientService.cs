using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;


namespace LikarKrapkaComUI.Services
{
    public class HubClientService
    {
            private string _baseAddress;

            public HubConnection connection;

            public HubClientService(string baseAddress)
            {
                _baseAddress = baseAddress;
            }

            public async Task ConnectAsync()
            {
                try
                {
                    if (connection != null && connection.State == HubConnectionState.Connected)
                        return;

                    connection = new HubConnectionBuilder().WithUrl(_baseAddress + "/ServerHub").Build();

                    await StartConnectionAsync();

                    connection.Closed += async (error) =>
                    {
                        await StartConnectionAsync();
                    };
                }
                catch
                {
                    return;
                }

            }
            private async Task StartConnectionAsync()
            {
                bool isHubConnect = false;
                while (!isHubConnect)
                {
                    try
                    {
                        await connection.StartAsync();
                        isHubConnect = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        isHubConnect = false;
                    }

                    Console.WriteLine(@"SignalR Hub Client Service: isHubConnect {0}", isHubConnect);
                }
            }

        }
    }
