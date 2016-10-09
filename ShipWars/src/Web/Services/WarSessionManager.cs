using System.Collections.Generic;
using System.Net.WebSockets;
using Web.Models;

namespace Web.Services
{
    public class WarSessionManager : IWarSessionManager
    {
        static List<WarSession> _sessions = new List<WarSession>();

        public void Add(WarSession session)
        {
            Remove(session.UserId);
            _sessions.Add(session);
        }

        public void Remove(string userId)
        {
            _sessions.RemoveAll(s => s.UserId == userId);
        }

        public void Clean() =>
            _sessions.RemoveAll(s => s.Socket.State == WebSocketState.Closed);

        public WarSession Find(string userId) => 
            _sessions.Find(s => s.UserId == userId && s.Socket.State == WebSocketState.Open);
    }
}
