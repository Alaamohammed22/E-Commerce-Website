using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Hubs
{
    public class CommentHub:Hub
    {
        UserManager<ApplicationUser> userManager;
        public CommentHub(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async void NewMessage(int supplierproductid,string name,string message,string imagename)
        {
           // ApplicationUser application = await userManager.FindByNameAsync(name);
            Clients.All.SendAsync("NewMessageNotify", supplierproductid, name,message, imagename);

        }
        public override Task OnConnectedAsync()
        {
            string name = "no Name";
            Clients.All.SendAsync("NewUser", name, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

    }
}
