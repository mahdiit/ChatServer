using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Pages
{
    public class IndexModel : PageModel
    {
        IHubContext<ChatHub> hubContext;
        public IndexModel(IHubContext<ChatHub>  hub)
        {
            hubContext = hub;
        }

        public void OnGet()
        {
            
        }
    }
}
