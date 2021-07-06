using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cichorium.Managers;

namespace Cichorium.Objects.ScreenObjects
{
    public class Label : ScreenObject
    {

        protected string text;
        public Color Color = new Color(76, 76, 76);
        protected float Alpha = 1f;
        public Point Size;
        public bool Background = false;

        public Label(string text)
        {
            this.text = text;
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public string GetText()
        {
            return text;
        }

        public Label(int x, int y, string text)
        {
            SetPosition(x, y);
            this.text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Background)
                spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], new Rectangle(Position, Size - (new Vector2(2, 2) * Scale).ToPoint()), Color.Black);
            //spriteBatch.DrawString(Cichorium.TextureManager.Fonts["text"], text, Position.ToVector2(), Color * Alpha, 0, Cichorium.TextureManager.Fonts["text"].MeasureString(text).ToPoint().ToVector2() / 2, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], text, Position.ToVector2(), Color * Alpha, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            Size = (Cichorium.TextureManager.Fonts["default"].MeasureString(text) * Scale).ToPoint();
            //ImageArea = new Rectangle(Container.Position + Position - ((texture.Bounds.Size.ToVector2() * Scale) / 2).ToPoint(), (texture.Bounds.Size.ToVector2() * Scale).ToPoint());

        }

    }
}
