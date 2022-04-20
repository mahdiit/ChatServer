using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Controllers
{
    //https://sahansera.dev/understanding-websockets-with-aspnetcore-5/
    [Route("api/[controller]")]
    [ApiController]
    public class wsController : ControllerBase
    {
        [HttpGet("GetService")]
        public async Task GetService()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {                    
                    await Echo(webSocket);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var msg = Encoding.UTF8.GetString(buffer);
                var serverMsg = Encoding.UTF8.GetBytes($"Server: Hello. You said: {msg}");

                await webSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    result.MessageType,
                    result.EndOfMessage, CancellationToken.None);
                
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        [HttpGet("ids")]
        public IActionResult GetIds()
        {
            return Ok(ChatServer.Hubs.ChatHub.ConnectedIds);
        }
    }
}
