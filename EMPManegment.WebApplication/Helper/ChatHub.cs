using EMPManagment.Web.Helper;
using Microsoft.AspNetCore.SignalR;

namespace EMPManegment.Web.Helper
{
    public class ChatHub : Hub
    {
        public ChatHub(WebAPI webAPI)
        {
            WebAPI = webAPI;
        }

        public WebAPI WebAPI { get; }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

