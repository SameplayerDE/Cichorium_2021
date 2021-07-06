using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cichorium.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Cichorium.Managers
{
    public class ScreenManager : DrawableGameComponent
    {

        new private Cichorium Game;
        private SpriteBatch spriteBatch;

        private readonly Stack<Screen> screens = new Stack<Screen>();

        public Screen GetCurrentScreen { get {
                if (screens.Count > 0) { return screens.Peek(); }
                else{ return null; } } }

        public ScreenManager(Cichorium game) : base(game)
        {
            this.Game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();

        }

        public void ShowScreen(Screen screen)
        {
            screens.Push(screen);
        }

        public void CloseScreen()
        {
            if (screens.Count > 0)
            {
                var screen = screens.Pop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(Cichorium.Instance.Resolution));
            var list = screens.ToArray();
            Array.Reverse(list);
            foreach (var screen in list)
                screen.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            var list = screens.ToArray();
            Array.Reverse(list);
            foreach (var screen in list)
                screen.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
