using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cichorium.Objects
{
    public class CollisionMask
    {

        public static readonly CollisionMask FULL = new CollisionMask(true, true, true, true);
        public static readonly CollisionMask HALF_LEFT = new CollisionMask(true, false, true, false);
        public static readonly CollisionMask HALF_RIGHT = new CollisionMask(false, true, false, true);

        public Rectangle Mask { get; private set; }

        // x1 x2 [x3 x4]
        // x3 x4

        public CollisionMask(bool x1, bool x2, bool x3, bool x4) : this(new bool[] {x1, x2, x3, x4}) {

        }

        private CollisionMask(bool[] mask)
        {
            int offsetX = 0;
            int offsetY = 0;
            int width = 0;
            int height = 0;

            if (mask[0] && mask[1] && mask[2] && mask[3])
            {
                width = 16;
                height = 16;
            }
            else
            {

                if (mask[0])
                {
                    width = 8;
                    height = 8;
                    if (mask[2])
                    {
                        height = 16;
                    }
                }
                else
                {
                    if (mask[1])
                    {
                        offsetX = 8;
                        width = 8;
                        height = 8;
                        if (mask[3])
                        {
                            height = 16;
                        }
                    }
                }
            }

            Mask = new Rectangle(offsetX, offsetY, width, height);

        }

    }
}
