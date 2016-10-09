using Web.Enums;

namespace Web.Services
{
    public class Ship
    {
        public static int GetMark(Player player, ShipType shipType) => 
            (int)player + (int)shipType;

        public static bool IsMine(Player player, int mark) => 
            GetPlayer(mark) == player;

        public static Player GetPlayer(int mark) =>
            mark < (int)Player.Player2 ? Player.Player1 : Player.Player2;
    }
}
