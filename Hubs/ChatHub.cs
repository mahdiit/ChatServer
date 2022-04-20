using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        IWebHostEnvironment environment;
        public ChatHub(IWebHostEnvironment  webHost)
        {
            environment = webHost;
        }

        public static HashSet<UserInfoDto> Users = new HashSet<UserInfoDto>();

        public override Task OnConnectedAsync()
        {
            Users.Add(new UserInfoDto() { ConnectionId = Context.ConnectionId });
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Users.Remove(Users.First(x => x.ConnectionId == Context.ConnectionId));
            await UserContactListUpdate();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task UserContactListUpdate()
        {
            await Clients.All.SendAsync("UpdateContactList", Users.Where(x => !string.IsNullOrEmpty(x.Name)).ToList());
        }

        public async Task Login(string connectionId, string name, string imageId)
        {            
            var user = Users.First(x => x.ConnectionId == connectionId);

            if (string.IsNullOrEmpty(imageId))
            {                
                var path = Path.Combine(environment.ContentRootPath, "wwwroot", "images");
                var files = Directory.GetFiles(path, "*.png");
                var rnd = new Random();
                var index = rnd.Next(1, files.Length);

                imageId = Path.GetFileNameWithoutExtension(files[index]);
            }

            user.Name = name;
            user.ImageUrl = imageId;
            await Clients.Client(connectionId).SendAsync("LoginSucess", user);
            await UserContactListUpdate();
        }

        public async Task SendMessage(UserMessageDto userMessage)
        {
            await Clients.All.SendAsync("ReceiveMessage", userMessage);
        }
    }
}
