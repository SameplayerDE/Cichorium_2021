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
    public abstract class Screen
    {

        protected bool DrawBackground = false;

        protected List<ScreenObject> Components = new List<ScreenObject>();
        public Point Position;
        public Point Size;
        public float Alpha = 1f;
        public Color Color = Color.White;
        public int ID;

        public MouseState latestMouseState, oldMouseState;
        public KeyboardState latestKeyboardState, oldKeyboardState;

        public Screen(int ID)
        {
            this.ID = ID;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DrawBackground)
                spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], new Rectangle(Position, Size), Color * Alpha);
            foreach (var obj in Components)
            {
                if (obj.Visible)
                    obj.Draw(spriteBatch);
            }
        }
        //public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void Update(GameTime gameTime)
        {

            latestMouseState = Mouse.GetState();
            latestKeyboardState = Keyboard.GetState();

            foreach (var obj in Components)
            {
                obj.Update(gameTime);
            }

            oldMouseState = latestMouseState;
            oldKeyboardState = latestKeyboardState;

        }
        //public abstract void Update(GameTime gameTime);

        public void Add(ScreenObject obj)
        {
            obj.Container = this;
            Components.Add(obj);
        }

        public void Remove(ScreenObject obj)
        {
            Components.Remove(obj);
        }

        public void RemoveAll()
        {
            Components.Clear();
        }

    }
}
