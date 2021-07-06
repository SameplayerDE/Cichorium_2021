using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace Cichorium.Objects
{
    public class GameSprite : Body
    {

        public Texture2D texture { get; }

        public float Depth { get; set; }
        public Color Color { get; set; }

        public Rectangle? DrawRect { get; set; }

        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }
        public float BodyHeight { get; protected set; }
        public float BodyWidth { get; protected set; }

        public GameSprite(string assetFile)
        {
            texture = Cichorium.TextureManager.Sprites[assetFile];
            Color = Color.White;
            BodyHeight = Height;
            BodyWidth = Width;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DrawRect == null)
                spriteBatch.Draw(texture, Position, null, Color.White, Rotation, texture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(texture, Position, DrawRect, Color.White, Rotation, DrawRect.Value.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0);

        }

        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
