using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cichorium.Managers
{
    public class HudManager : DrawableGameComponent
    {

        new private Cichorium Game;
        private SpriteBatch spriteBatch;
        private ContentManager ContentManager;

        private Texture2D heart_empty;
        private Texture2D heart_full;

        private Texture2D[] action_button = new Texture2D[2];

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }
        public bool InputAwaiting { get; set; } = false;

        private int W = 1;

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        public HudManager(Cichorium game) : base(game)
        {
            Game = game;
            
            ContentManager = Game.Content;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            heart_empty = ContentManager.Load<Texture2D>("heart_empty");
            heart_full = ContentManager.Load<Texture2D>("heart_full");

            action_button[0] = ContentManager.Load<Texture2D>("a_button");
            action_button[1] = ContentManager.Load<Texture2D>("enter_key");

            base.LoadContent();

        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(Cichorium.Instance.Resolution));
            DrawHealthBar();
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(Cichorium.Instance.Resolution));

            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2 - W, 15 - W), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2, 15 - W), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2 + W, 15 - W), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2 + W, 15), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2 + W, 15 + W), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2, 15 + W), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2 - W, 15 + W), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2 - W, 15), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            spriteBatch.DrawString(Cichorium.TextureManager.Fonts["default"], "FPS: " + Math.Round(1 / (double)gameTime.ElapsedGameTime.TotalSeconds, 0) + "", new Vector2(2, 15), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawHealthBar()
        {
            int offsetX = 2;
            int offsetY = 2;
            int heartWidth = 9;

            for (int i = 0; i < SaveFileManager.Data.HealthScale; i++)
            {
                spriteBatch.Draw(heart_empty, new Vector2(heartWidth * i + offsetX + i, offsetY), Color.White);
            }

            for (int i = 0; i < SaveFileManager.Data.Health; i++)
            {
                spriteBatch.Draw(heart_full, new Vector2(heartWidth * i + offsetX + i, offsetY), Color.White);
            }

        }

        public void DrawInputNotification()
        {
            if (InputAwaiting)
                spriteBatch.Draw(action_button[1], Vector2.Zero, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
