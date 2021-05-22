using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace RoutePlannerApi.Hubs
{
    public class RoutesHub : Hub
    {
        public async Task SendRoutesHaveChanged()
        {
            await Clients.Others.SendAsync("RoutesChanged");
        }
    }
}