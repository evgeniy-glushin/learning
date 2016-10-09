using Microsoft.AspNetCore.Mvc;
using Web.ApplicationContext;
using Web.Services;

namespace Web.Controllers
{
    public class NotificationsController : Controller
    {
        public bool Ping([FromServices]NotificationsService notifServ,
                         [FromServices]ICurrentUser player)
        {
            return notifServ.Exists(player.UserId);
        }
    }
}
