using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cichorium.Objects
{
    public abstract class ScreenObject
    {

        public Screen Container;
        public Point Position = new Point(0, 0);
        public float Scale = 1;
        public bool Visible = true;
        public bool Selected = false;

        public void SetPosition(int x, int y)
        {
            Position = new Point(x, y);
        }

        public void SetPosition(Point Position)
        {
            this.Position = Position;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

    }
}
