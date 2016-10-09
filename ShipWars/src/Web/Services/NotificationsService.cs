using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.Services
{
    public class NotificationsService
    {
        static List<Notification> _notifications = new List<Notification>();
        public void Add(Notification notification) => 
            _notifications.Add(notification);
        
        public bool Exists(string userId)
        {
            if (_notifications.Any(n => n.UserId == userId))
            {
                Remove(userId);
                return true;
            }
            else
                return false;
        }

        public void Remove(string userId) =>
            _notifications.RemoveAll(n => n.UserId == userId);
    }
}
