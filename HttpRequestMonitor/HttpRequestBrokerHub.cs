using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpRequestMonitor
{
    public class HttpRequestBrokerHub : Hub
    {
        public async Task HttpRequestReceived(string url, string method, Dictionary<string, string[]> headers, string body)
        {
            await Clients.All.SendAsync("HttpRequestReceived", url, method, headers, body);
        }
    }
}
