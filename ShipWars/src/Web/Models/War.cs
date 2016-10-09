using Web.Enums;

namespace Web.Models
{
    public class War : Entity
    {
        public string Player1Id { get; set; }

        /// <summary>
        /// The player who is inviter.
        /// </summary>
        public User Player1 { get; set; }

        public string Player2Id { get; set; }

        /// <summary>
        /// The player who was invited to war.
        /// </summary>
        public User Player2 { get; set; }

        public WarStatus Status { get; set; }
        
        public int Shots1 { get; set; }

        public int Shots2 { get; set; }

        /// <summary>
        /// The score of Player1.
        /// </summary>
        public int Score1 { get; set; }

        /// <summary>
        /// The score of Player2.
        /// </summary>
        public int Score2 { get; set; }

        public int WinScore { get; set; }

        public string WhoShotId { get; set; }
        public User WhoShot { get; set; }

        public string WinnerId { get; set; }
        public User Winner { get; set; }

        public string Field { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
    }
}
