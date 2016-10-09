using Web.Models;

namespace Web.ViewModels
{
    public class ActiveWarVm
    {
        public int Id { get; set; }

        /// <summary>
        /// The score of Player1.
        /// </summary>
        public int Score1 { get; set; }

        /// <summary>
        /// The score of Player2.
        /// </summary>
        public int Score2 { get; set; }

        public int Shots1 { get; set; }

        public int Shots2 { get; set; }
        
        public Section[,] Field { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public string Player1Nickname { get; set; }
        public string Player2Nickname { get; set; }

        public bool IsFinished { get; internal set; }
        public string GameOverMsg { get; internal set; }
        public bool IsShotTime { get; internal set; }
    }
}
