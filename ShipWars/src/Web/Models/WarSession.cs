using System.Net.WebSockets;

namespace Web.Models
{
    public class WarSession
    {        
        public WarSession(string userId, WebSocket socket)
        {
            UserId = userId;
            Socket = socket;
        }

        public string UserId { get; set; }
        public WebSocket Socket { get; set; }
    }
}
