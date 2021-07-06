using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cichorium.Objects
{
    public class DragableScreen : Screen
    {

        private Point Anchor;
        private bool fixedBackground = true;

        public DragableScreen(int ID) : base(ID)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (DrawBackground)
            {
                if (fixedBackground)
                    spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], new Rectangle(new Point(0, 0), Size), Color * Alpha);
                else
                    spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], new Rectangle(Position, Size), Color * Alpha);
            }
        }

        public void DrawObjects(SpriteBatch spriteBatch)
        {
            foreach (var obj in Components)
            {
                if (obj.Visible)
                    obj.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            latestMouseState = Mouse.GetState();
            latestKeyboardState = Keyboard.GetState();


            if (latestMouseState.MiddleButton == ButtonState.Pressed)
            {
                Anchor = (oldMouseState.Position.ToVector2() / Cichorium.Instance.Resolution).ToPoint();
                Position += ((latestMouseState.Position.ToVector2() / Cichorium.Instance.Resolution).ToPoint() - Anchor);
            }

            if (Cichorium.InputManager.UsingGamePad)
            {
                Position += (new Vector2(-Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X, Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y) * 4).ToPoint();
            }

            foreach (var obj in Components)
            {
                obj.Update(gameTime);
            }

            oldMouseState = latestMouseState;
            oldKeyboardState = latestKeyboardState;
            
        }
    }
}
