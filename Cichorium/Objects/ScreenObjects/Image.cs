using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cichorium.Objects.ScreenObjects
{
    public class Image : ScreenObject
    {

        protected Texture2D texture;
        protected Color Color = Color.White;
        protected float Alpha = 1f;
        protected Rectangle ImageArea;
        protected float Rotation = 0f;
        protected float Scale = 1;

        public Image(string source)
        {
            //Scale = Cichorium.Instance.Resolution;
            texture = Cichorium.TextureManager.Sprites[source];
        }

        public void SetSource(string source)
        {
            texture = Cichorium.TextureManager.Sprites[source];
        }

        public Image(int x, int y, string source)
        {
            Position = new Point(x, y);
            texture = Cichorium.TextureManager.Sprites[source];
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, ImageArea, Color * Alpha);
            if (Container != null)
            {
                spriteBatch.Draw(texture, Container.Position.ToVector2() + Position.ToVector2(), null, Color * Alpha, Rotation, texture.Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(texture, Position.ToVector2(), null, Color * Alpha, Rotation, texture.Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
            }  
        }

        public override void Update(GameTime gameTime)
        {
            if (Container != null)
                ImageArea = new Rectangle((Container.Position.ToVector2() * Cichorium.Instance.Resolution + Position.ToVector2() * Cichorium.Instance.Resolution).ToPoint() - ((texture.Bounds.Size.ToVector2() * Cichorium.Instance.Resolution) / 2).ToPoint(), (texture.Bounds.Size.ToVector2() * Cichorium.Instance.Resolution).ToPoint());

        }

    }
}
