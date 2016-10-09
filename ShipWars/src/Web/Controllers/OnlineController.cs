using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Enums;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class OnlineController : Controller
    {
        private UserManager<User> _userMgr;
        private NotificationsService _notifServ;

        public OnlineController(UserManager<User> userMgr,
            NotificationsService notifServ)
        {
            _userMgr = userMgr;
            _notifServ = notifServ;
        }

        [HttpPost]
        public async Task<StatusCodeResult> Yes()
        {
            var user = await _userMgr.GetUserAsync(User);
            if(!user.IsOnline)
            {
                user.IsOnline = true;
                await _userMgr.UpdateAsync(user);

                CreateNotification(user, NotificationType.NewOnlineUser);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<StatusCodeResult> No()
        {
            var user = await _userMgr.GetUserAsync(User);
            if (user.IsOnline)
            {
                user.IsOnline = false;
                await _userMgr.UpdateAsync(user);

                CreateNotification(user, NotificationType.UserHasLeftApp);
            }

            return Ok();
        }


        private void CreateNotification(User user, NotificationType type)
        {
            var onlineUsers = _userMgr.Users
                .Where(u => u.IsOnline && u.Id != user.Id)
                .Select(u => u.Id)
                .ToList();

            onlineUsers.ForEach(id =>
                _notifServ.Add(new Notification(id, type)));
        }
    }
}
