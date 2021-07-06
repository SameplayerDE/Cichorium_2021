using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cichorium.Managers;
using tainicom.Aether.Physics2D.Dynamics;

namespace Cichorium.Objects
{
    public class Tile : Body
    {

        public static List<Tile> Tiles = new List<Tile>();

        public int ID { get; private set; }
        public Rectangle Mask { get { return mask.Mask; }}

        private CollisionMask mask;

        public Tile(int id, CollisionMask mask)
        {
            ID = id;
            this.mask = mask;
        }

    }

}
