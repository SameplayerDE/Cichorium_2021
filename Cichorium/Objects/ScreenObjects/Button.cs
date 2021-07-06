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
    public class Button : ScreenObject
    {
        protected string text;
        public Color TextColor = new Color(76, 76, 76);
        //public Color TextColor = Color.Black * 0.5f;
        public Color BackgroundColor = Color.LightGray;
        public Color BorderColor = Color.Black * 0.5f;
        protected float Alpha = 1f;
        public Point Size;
        private Rectangle ButtonArea;

        public int Width = 1, Height = 1;

        public bool Clicked = false;
        public bool Hover = false;

        public Button(string text)
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

        public Button(int x, int y, string text)
        {
            SetPosition(x, y);
            this.text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], ButtonArea, BorderColor);

            string var = "button";
            Point dimensions = Cichorium.TextureManager.Sprites[var + "-0"].Bounds.Size;

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-0"], new Rectangle(Position.X, Position.Y, dimensions.X, dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-1"], new Rectangle(Position.X + dimensions.X, Position.Y, Width - (dimensions.X * 2), dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-2"], new Rectangle(Position.X + Width - dimensions.X, Position.Y, dimensions.X, dimensions.Y), BackgroundColor);

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-3"], new Rectangle(Position.X, Position.Y + dimensions.Y, dimensions.X, Height - (dimensions.Y * 2)), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-4"], new Rectangle(Position.X + dimensions.X, Position.Y + dimensions.Y, Width - (dimensions.X * 2), Height - (dimensions.Y * 2)), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-5"], new Rectangle(Position.X + Width - dimensions.X, Position.Y + dimensions.Y, dimensions.X, Height - (dimensions.Y * 2)), BackgroundColor);

            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-6"], new Rectangle(Position.X, Position.Y + dimensions.Y + Height - (dimensions.Y * 2), dimensions.X, dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-7"], new Rectangle(Position.X + dimensions.X, Position.Y + dimensions.Y + Height - (dimensions.Y * 2), Width - (dimensions.X * 2), dimensions.Y), BackgroundColor);
            spriteBatch.Draw(Cichorium.TextureManager.Sprites[var + "-8"], new Rectangle(Position.X + Width - dimensions.X, Position.Y + Height - dimensions.Y, dimensions.X, dimensions.Y), BackgroundColor);

            //spriteBatch.DrawString(Cichorium.TextureManager.Fonts["text"], text, Position.ToVector2(), Color * Alpha, 0, Cichorium.TextureManager.Fonts["text"].MeasureString(text).ToPoint().ToVector2() / 2, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], text, ButtonArea.Center.ToVector2() - (Size.ToVector2() / 2), TextColor * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {

            Scale = Cichorium.Instance.Resolution;

            if (!Cichorium.InputManager.UsingGamePad)
            {
                if (ButtonArea.Contains(Cichorium.InputManager.CurrentMouseState.Position.ToVector2() / Scale))
                {
                    Hover = true;
                    if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.LeftClicked))
                    {
                        Clicked = true;
                        Cichorium.AudioManager.SoundEffects["button_interact"].Play();
                    }
                }
                else
                {
                    Hover = false;
                    if (Clicked) Clicked = false;
                }
            }
            else
            {
                Hover = Selected;
                if (Selected)
                {
                    if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.MenuYes))
                    {
                        Clicked = true;
                        Cichorium.AudioManager.SoundEffects["button_interact"].Play();
                    }
                }
                else
                {
                    if (Clicked) Clicked = false;
                }
            }
            
            if (Hover)
            {
                BackgroundColor = new Color(23, 205, 7);
                TextColor = Color.White;
                BorderColor = Color.White;
            }
            else
            {
                BackgroundColor = Color.LightGray;
                TextColor = new Color(76, 76, 76);
               //TextColor = Color.White;
                BorderColor = Color.Black;
            }

            Size = Cichorium.TextureManager.Fonts["default"].MeasureString(text).ToPoint();
            
            ButtonArea = new Rectangle(Position, new Point(Width, Height));
            //ButtonArea = new Rectangle(Position, new Point(Width * Scale + (14 * Scale), Height * Scale + (14 * Scale)));
            //ButtonArea = new Rectangle((Position.ToVector2() * Cichorium.Instance.Resolution).ToPoint(), new Point(Width * Scale + (14 * Scale), Height * Scale + 14));
        }
    }
}
