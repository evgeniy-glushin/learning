using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Web.ApplicationContext;
using Web.Models;
using Web.ViewModels;

namespace Web.Services
{
    public class WarObserver : IObserver<ActiveWarVm>
    {
        private IActiveWar _activeWar;
        private IWarSessionManager _warSessionManager;

        public WarObserver(IActiveWar activeWar, IWarSessionManager warSessionManager)
        {
            _activeWar = activeWar;
            _warSessionManager = warSessionManager;
        }


        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ActiveWarVm value)
        {
            var warSession = _warSessionManager.Find(_activeWar.CompetitorId);
            var socket = warSession.Socket;
            if (socket.State == WebSocketState.Open)
            {
                var json = JsonConvert.SerializeObject(value);
                var data = Encoding.UTF8.GetBytes(json);
                var buffer = new ArraySegment<byte>(data);

                socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }
        }
    }
}
