using System;
using System.Collections.Generic;
using System.Linq;
using Web.Enums;
using Web.Models;

namespace Web.Services
{
    public class FieldBuilder
    {
        public FieldBuilder(GameConfig config)
        {
            _config = config;
            _countSttings = new Dictionary<ShipType, int>
            {
                { ShipType.OnePiece, _config.OnePieceShipsCount },
                { ShipType.TwoPiece, _config.TwoPieceShipsCount },
                { ShipType.ThreePiece, _config.ThreePieceShipsCount }
            };
        }

        Dictionary<ShipType, int> _countSttings;
        Random _random = new Random();
        Section[,] _field;
        GameConfig _config;

        public Section[,] Build()
        {
            _field = new Section[_config.Rows, _config.Cols];
            foreach (var item in _countSttings)
            {
                var shipsCount = item.Value;
                for (int i = 0; i < shipsCount; i++)
                {
                    var shipType = item.Key;
                    if (IsOdd(i))
                        PopulateField(Player.Player1, shipType);
                    else
                        PopulateField(Player.Player2, shipType);
                }
            }
            return _field;
        }
        private bool IsOdd(int i) => i % 2 != 0;
        private void PopulateField(Player player, ShipType shipType)
        {
            var a = _random.Next(0, _config.Rows);
            var b = Enumerable.Range(_random.Next(_config.Rows), (int)shipType);

            IEnumerable<Position> ship;
            if (IsTrue)
                ship = b.Select(x => new Position { Row = a, Col = x });
            else
                ship = b.Select(x => new Position { Row = x, Col = a });

            if (IsValid(ship))
                ship.ToList()
                    .ForEach(p => _field[p.Row, p.Col] = new Section(Ship.GetMark(player, shipType), SectionState.Hidden));
            else
                PopulateField(player, shipType);
        }

        /// <summary>
        /// Gets random bool value.
        /// </summary>
        public bool IsTrue => _random.Next(2000) < 1000 ? true : false;

        //private int ShipMark(int player, ShipType shipType) => player + (int)shipType;

        private bool IsValid(IEnumerable<Position> ship)
        {
            var isShipInFieldBounds = ship.All(x => x.Col >= 0 && x.Col < _config.Cols &&
                                                    x.Row >= 0 && x.Row < _config.Rows);
            if (!isShipInFieldBounds) return false;

            var isSpaceForShipFree = ship.All(x => !IsBusyCell(x));
            return isSpaceForShipFree;
        }
        private bool IsBusyCell(Position pos) => _field[pos.Row, pos.Col] != null;

        private struct Position
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public static Position Random(int rows, int cols)
            {
                Random random = new Random();
                return new Position
                {
                    Row = random.Next(0, rows),
                    Col = random.Next(0, cols)
                };
            }
        }
    }
}
