using Microsoft.Extensions.Configuration;

namespace Web.Services
{
    public class GameConfig
    {
        private IConfigurationRoot _config;

        public GameConfig(IConfigurationRoot config)
        {
            _config = config;
        }

        public int Rows => int.Parse(_config["GameSettings:Field:Rows"]);

        public int Cols => int.Parse(_config["GameSettings:Field:Cols"]);

        public int OnePieceShipsCount => int.Parse(_config["GameSettings:ShipsCount:OnePiece"]);
        public int TwoPieceShipsCount => int.Parse(_config["GameSettings:ShipsCount:TwoPiece"]);
        public int ThreePieceShipsCount => int.Parse(_config["GameSettings:ShipsCount:ThreePiece"]);
    }
}
