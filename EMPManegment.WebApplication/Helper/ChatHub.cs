using Microsoft.AspNetCore.SignalR;

namespace EMPManegment.Web.Helper
{
    public class ChatHub : Hub
    {
        public ChatHub() { }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

