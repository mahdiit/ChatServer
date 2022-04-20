using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Hubs
{
    public class UserInfoDto
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    }
}
