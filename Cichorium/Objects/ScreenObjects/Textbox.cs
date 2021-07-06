using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cichorium.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cichorium.Objects.ScreenObjects
{
    public class Textbox : ScreenObject
    {

        public static int Padding_Top = 7;
        public static int Padding_Bottom = 7;
        public static int Padding_Left = 7;
        public static int Padding_Right = 7;

        private int Width = 100, Height = 100;
        private Color BackgroundColor = Color.White;
        public string Message = "Empty";

        public Textbox(int x, int y, int width, int height, bool calculatePadding, bool center, bool CenterAsCenter)
        {
            
            if (calculatePadding)
            {
                Position = new Point(x - Padding_Left, y - Padding_Top);
                Width = Padding_Left + width + Padding_Right;
                Height = Padding_Top + height + Padding_Bottom;
            }
            else
            {
                Position = new Point(x, y);
                Width = width;
                Height = height;
            }

            if (center)
            {
                Position = new Point(Padding_Left + Cichorium.Middle.X - Width / 2, Padding_Right + Cichorium.Middle.Y - Height / 2);
            }

            if (CenterAsCenter)
            {
                Position = new Point(Padding_Left + Cichorium.Middle.X - Width / 2, Padding_Right + Cichorium.Middle.Y - Height / 2);
                Position = (Position.ToVector2() + new Vector2(x, y)).ToPoint();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            string var = "textbox2";
            Point dimensions = Cichorium.TextureManager.Sprites[var + "-0"].Bounds.Size;

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-0"], new Rectangle(Position.X - dimensions.X, Position.Y - dimensions.Y, dimensions.X, dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-1"], new Rectangle(Position.X, Position.Y - dimensions.Y, Width - (dimensions.X * 2), dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-2"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y - dimensions.Y, dimensions.X, dimensions.Y), BackgroundColor);

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-3"], new Rectangle(Position.X - dimensions.X, Position.Y, dimensions.X, Height - (dimensions.Y * 2)), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-4"], new Rectangle(Position.X, Position.Y, Width - (dimensions.X * 2), Height - (dimensions.Y * 2)), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-5"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y, dimensions.X, Height - (dimensions.Y * 2)), BackgroundColor);

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-6"], new Rectangle(Position.X - dimensions.X, Position.Y + Height - (dimensions.Y * 2), dimensions.X, dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-7"], new Rectangle(Position.X, Position.Y + Height - (dimensions.Y * 2), Width - (dimensions.X * 2), dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-8"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y + Height - (dimensions.Y * 2), dimensions.X, dimensions.Y), BackgroundColor);

            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], Message, Position.ToVector2() + new Vector2(Padding_Left + 1, Padding_Bottom + 1) / 2, Color.White * 1f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

        }

        public override void Update(GameTime gameTime)
        {
            Scale = Cichorium.Instance.Resolution;
        }
    }
}
