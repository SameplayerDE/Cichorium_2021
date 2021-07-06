using Cichorium.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cichorium.Managers
{
    public class TileManager : GameComponent
    {

        private new Cichorium Game;

        public TileManager(Cichorium game) : base(game)
        {
            Game = game;
        }

        public override void Initialize()
        {

            //Walls in house

            Tile.Tiles.Add(new Tile(22, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(23, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(24, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(37, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(38, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(40, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(50, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(51, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(56, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(66, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(67, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(69, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(85, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(97, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(98, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(101, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(102, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(104, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(118, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(120, CollisionMask.FULL));

            //House outside

            Tile.Tiles.Add(new Tile(25, CollisionMask.HALF_RIGHT));
            Tile.Tiles.Add(new Tile(26, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(27, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(28, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(29, CollisionMask.HALF_LEFT));
            Tile.Tiles.Add(new Tile(41, CollisionMask.HALF_RIGHT));
            Tile.Tiles.Add(new Tile(42, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(43, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(44, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(45, CollisionMask.HALF_LEFT));
            Tile.Tiles.Add(new Tile(57, CollisionMask.HALF_RIGHT));
            Tile.Tiles.Add(new Tile(58, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(60, CollisionMask.FULL));
            Tile.Tiles.Add(new Tile(61, CollisionMask.HALF_LEFT));


            base.Initialize();
        }
    }
}
