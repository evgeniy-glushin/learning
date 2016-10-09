using Web.Enums;

namespace Web.Models
{
    public class Invitation : Entity
    {
        public string InviterId { get; set; }
        public User Inviter { get; set; }

        public string InvitedId { get; set; }
        public User Invited { get; set; }

        public InvitationStatus Status { get; set; }
        public bool Seen { get; internal set; }
    }
}
