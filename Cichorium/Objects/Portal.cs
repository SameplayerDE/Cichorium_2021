using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace Cichorium.Objects
{
    public class Portal : Body
    {

        public string Destination { get; set; }
        public Rectangle BoundingBox { get; set; }

        public Portal(string destination, Rectangle boundingBox)
        {
            Destination = destination;
            BoundingBox = boundingBox;
            CreateRectangle(BoundingBox.Width, BoundingBox.Height, 1f, Vector2.Zero);
            Position = BoundingBox.Location.ToVector2();
            SetIsSensor(true);
            BodyType = BodyType.Kinematic;
        }

    }
}
