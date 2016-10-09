using Web.Enums;

namespace Web.Models
{
    public class Notification
    {
        public Notification(string userId, NotificationType type)
        {
            UserId = userId;
            Type = type;
        }
        public NotificationType Type { get; private set; }
        public string UserId { get; private set; }
    }
}
