using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Web.ApplicationContext;
using Web.Enums;
using Web.Models;
using Web.Services;
using Web.ViewModels;
using static Web.Enums.RespMsgState;

namespace Web.Middlewares
{
    public class WebSocketMiddleware
    {
        private RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context,
            IWarSessionManager sessionManager,
            UserManager<User> userManager,
            WarService warService,
            IActiveWar activeWar)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var requestSocket = await context.WebSockets.AcceptWebSocketAsync();
                var currentUserId = userManager.GetUserId(context.User);

                var newWarSession = new WarSession(currentUserId, requestSocket);
                sessionManager.Add(newWarSession);

                while (requestSocket.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var received = await requestSocket.ReceiveAsync(buffer, CancellationToken.None);
                    var competitorId = activeWar.CompetitorId;

                    switch (received.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            var receivedJson = Encoding.UTF8.GetString(buffer.Array,
                                buffer.Offset,
                                buffer.Count);

                            var shotParam = JsonConvert.DeserializeObject<ShotVm>(receivedJson);
                            var shotResult = await warService.ShotAsync(shotParam);

                            var warModel = shotResult.Item1;
                            var errorMsg = shotResult.Item2;

                            var currentPlayerRespMsg = new SocketRespMsg
                            {
                                Content = warModel,
                                ErrorMessage = errorMsg,
                                State = errorMsg == null ? Success : Failure
                            };
                            await SendAsync(requestSocket, currentPlayerRespMsg);

                            if (currentPlayerRespMsg.State == Success)
                            {
                                var competotorSession = sessionManager.Find(competitorId);
                                if (competotorSession != null)
                                {
                                    var competotorSocket = competotorSession.Socket;
                                    if (competotorSocket.State == WebSocketState.Open)
                                    {
                                        var competitorWarModel = await warService.FindAsync(competitorId);
                                        await SendAsync(competotorSocket, new SocketRespMsg { Content = competitorWarModel, State = Success });
                                    }
                                }
                            }                           

                            break;
                    }
                }

                await Task.FromResult(0);
            }
            else
            {
                await _next(context);
            }
        }

        static JsonSerializer _serializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        private async Task SendAsync(WebSocket requestSocket, SocketRespMsg message)
        {
            var jsonToSend = JObject.FromObject(message, _serializer).ToString();
            var dataToSend = Encoding.UTF8.GetBytes(jsonToSend);
            ArraySegment<byte> buffer = new ArraySegment<byte>(dataToSend);
            await requestSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
