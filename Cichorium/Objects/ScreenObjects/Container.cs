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
    public class Container : ScreenObject
    {

        public static int Padding_Top = 7;
        public static int Padding_Bottom = 7;
        public static int Padding_Left = 7;
        public static int Padding_Right = 7;

        private int Width = 200, Height = 100;
        private Color BackgroundColor = Color.LightGray * 1f;
        private List<ScreenObject> ScreenObjects = new List<ScreenObject>();
        private int CurrentItem = 0;

        public Container(int x, int y, int width, int height, bool calculatePadding, bool center)
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
        }

        public void Add(ScreenObject screenObject)
        {

            screenObject.SetPosition((screenObject.Position.ToVector2() + Position.ToVector2()).ToPoint());
            
            if (screenObject is Button)
            {
                Button button = screenObject as Button;
                button.Width = Width - 14;
                button.Height = 20;
            }

            //screenObject.Scale = Scale;
            ScreenObjects.Add(screenObject);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            string var = "panel";
            Point dimensions = Cichorium.TextureManager.Sprites[var + "-0"].Bounds.Size;

            //Shadow
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-6"], new Rectangle(Position.X - dimensions.X, Position.Y + Height - (dimensions.Y * 2) + 4, dimensions.X, dimensions.Y), Color.Black * 0.5f);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-7"], new Rectangle(Position.X, Position.Y + Height - (dimensions.Y * 2) + 4, Width - (dimensions.X * 2), dimensions.Y), Color.Black * 0.5f);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-8"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y + Height - (dimensions.Y * 2) + 4, dimensions.X, dimensions.Y), Color.Black * 0.5f);

            //Texture
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-0"], new Rectangle(Position.X - dimensions.X, Position.Y - dimensions.Y, dimensions.X, dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-1"], new Rectangle(Position.X, Position.Y - dimensions.Y, Width - (dimensions.X * 2), dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-2"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y - dimensions.Y, dimensions.X, dimensions.Y), BackgroundColor);

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-3"], new Rectangle(Position.X - dimensions.X, Position.Y, dimensions.X, Height - (dimensions.Y * 2)), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-4"], new Rectangle(Position.X, Position.Y, Width - (dimensions.X * 2), Height - (dimensions.Y * 2)), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-5"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y, dimensions.X, Height - (dimensions.Y * 2)), BackgroundColor);

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-6"], new Rectangle(Position.X - dimensions.X, Position.Y + Height - (dimensions.Y * 2), dimensions.X, dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-7"], new Rectangle(Position.X, Position.Y + Height - (dimensions.Y * 2), Width - (dimensions.X * 2), dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-8"], new Rectangle(Position.X + Width - (dimensions.X * 2), Position.Y + Height - (dimensions.Y * 2), dimensions.X, dimensions.Y), BackgroundColor);

            //Content
            foreach (var obj in ScreenObjects)
            {
                if (obj.Visible)
                    obj.Draw(spriteBatch);
            }

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var obj in ScreenObjects)
            {
                obj.Update(gameTime);
                obj.Selected = false;
            }

            if (Cichorium.InputManager.UsingGamePad)
            {
                if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.MenuDown))
                {
                    if (CurrentItem >= 0 && CurrentItem < ScreenObjects.Count - 1)
                    {
                        CurrentItem += 1;
                    }
                }
                if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.MenuUp))
                {
                    if (CurrentItem > 0 && CurrentItem <= ScreenObjects.Count - 1)
                    {
                        CurrentItem -= 1;
                    }
                }
                if (ScreenObjects.Any())
                {
                    ScreenObjects[CurrentItem].Selected = true;
                }
            }
        }
    }
}
