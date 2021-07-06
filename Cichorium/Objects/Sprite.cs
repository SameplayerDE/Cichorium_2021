using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using Cichorium.Managers;

namespace Cichorium.Objects
{
    public class Sprite : Body
    {

        public Texture2D Texture;
        protected bool Invisible = false;
        public float Scale = 1f;
        public Color Color = Color.White;
        public float Alpha = 1f;
        public float Depth = 1f;
        public Rectangle? DrawRect { get; set; }

        public Sprite(bool invisible, float x, float y)
        {
            Invisible = invisible;
            Position = new Vector2(x, y);
        }

        public Sprite(string file, float x, float y)
        {
            Texture = Cichorium.TextureManager.Sprites[file];
            Position = new Vector2(x, y);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Invisible)
            {
                if (DrawRect == null)
                    spriteBatch.Draw(Texture, Position, null, Color * Alpha, Rotation, Texture.Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(Texture, Position, DrawRect, Color * Alpha, Rotation, DrawRect.Value.Size.ToVector2() / 2, Scale, SpriteEffects.None, 0);
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
