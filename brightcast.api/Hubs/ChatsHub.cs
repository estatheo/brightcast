using brightcast.Models.Chats;  
using Microsoft.AspNetCore.SignalR;  
using System.Threading.Tasks;  
  
namespace ChatApp.Hubs  
{  
    public class ChatHub : Hub  
    {  
        public async Task NewMessage(ChatModel msg)  
        {  
            await Clients.All.SendAsync("messageReceived", msg);  
        }  
    }  
}