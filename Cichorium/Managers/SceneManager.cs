using Cichorium.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Common;

namespace Cichorium.Managers
{
    public class SceneManager : DrawableGameComponent
    {

        public bool Pause = false;
        new private Cichorium Game;
        private SpriteBatch spriteBatch;
        public static Camera Camera { get; private set; }
        public Region CurrentRegion { get; private set; }
        private ContentManager ContentManager;
        private Animation Water;

        Effect shader;

        Player player;

        public SceneManager(Cichorium game) : base(game)
        {
            Game = game;
            GraphicsDevice graphics = Game.GraphicsDevice;
            Camera = new Camera(graphics.Viewport);
            Camera.Scale = (int)(1 * ((float)graphics.Viewport.Width / 240));
            ContentManager = Game.Content;
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            shader = ContentManager.Load<Effect>("Shader");
            Water = new Animation("Sprites/Water.png", 24, 24, new TimeSpan(0, 0, 0, 0, 125));

            base.LoadContent();
            
        }

        public void SetRegion(Region region)
        {
            if (Cichorium.SimulationManager.CurrentRegion != region)
            {
                Cichorium.SimulationManager.CurrentRegion = region;
            }

            if (region.Startposition.Count > 0)
            {
                if (player != null)
                {
                    player = new Player(0, 0) { Direction = player.Direction };
                }
                else
                {
                    player = new Player(0, 0);
                }
                
                if (CurrentRegion != null)
                {
                    Console.WriteLine(CurrentRegion.Title);
                    player.Position = region.Startposition[CurrentRegion.Title];
                    Cichorium.AudioManager.SoundEffects["region_change"].Play(0.5f, 0f, 0f);
                }
                else
                {
                    if (region.Startposition.ContainsKey(region.Title))
                        player.Position = region.Startposition[region.Title];
                    else
                        player.Position = SaveFileManager.Data.Position.ToVector2();
                }
                Camera.Focus = player.WorldCenter;
                region.AddBody(player);
                region.AddBody(player.secondBody);
            }

            CurrentRegion = region;
            Console.WriteLine(CurrentRegion.Width);
            Console.WriteLine(CurrentRegion.Height);
            Console.WriteLine(CurrentRegion.TileSize);
            
            Camera.Focus = player.WorldCenter;

        }

        public void DrawSprite(Sprite sprite)
        {
            sprite.Draw(spriteBatch);
        }

        public void RenderBody(Body body)
        {
            spriteBatch.Draw(Cichorium.Missing, body.Position, null, Color.White, body.Rotation, Vector2.Zero, 5f, SpriteEffects.None, 1);
        }

        public void RenderLayer(Layer layer)
        {
            int TileSize = CurrentRegion.TileSize;
            
            for (int x = 0; x < CurrentRegion.Width; x++)
            {

                for (int y = 0; y < CurrentRegion.Height; y++)
                {

                    int tileID = layer.Data[x, y];
                    if (tileID == 0)
                    {
                        continue;
                    }

                    tileID -= 1;

                    if (tileID == 512)
                    {
                        spriteBatch.Draw(Water.Frames[Water.Frame], new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), null, Color.White);
                    }
                    else
                    {

                        Texture2D tileTexture = Cichorium.TextureManager.Sprites["tiles"];
                        Rectangle tileRectangle = new Rectangle(
                            TileSize * (tileID % (tileTexture.Width / TileSize)),
                            TileSize * (tileID / (tileTexture.Height / TileSize)),
                            TileSize,
                            TileSize);

                        spriteBatch.Draw(tileTexture, new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), tileRectangle, Color.White);
                    }
                }
            }
            
        }

        public override void Draw(GameTime gameTime)
        {

            if (!Enabled)
            {
                return;
            }

            if (CurrentRegion != null)
            {
                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform, rasterizerState: RasterizerState.CullNone, blendState: BlendState.AlphaBlend);
                int i = 0;
                foreach (Layer layer in CurrentRegion.Layers)
                {
                    if (layer.IsVisible) {
                        RenderLayer(layer);
                    }
                    i++;
                    if (i == 2)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            foreach (Body body in CurrentRegion.World.BodyList)
                            {
                                if (body is Sprite)
                                {
                                    Sprite sprite = body as Sprite;

                                    if (sprite.Depth != j)
                                    {
                                        continue;
                                    }

                                    DrawSprite(sprite);
                                }
                            }
                        }
                    }
                }
                foreach (Body b in CurrentRegion.Bodies)
                {

                    if (b is CollisionObject)
                    {
                        Color c = Color.Orange;
                        CollisionObject collisionObject = b as CollisionObject;
                        //spriteBatch.Draw(Cichorium.Missing, b.Position, null, c, b.Rotation, Vector2.Zero, new Vector2(collisionObject.Width, collisionObject.Height) / Cichorium.Missing.Bounds.Size.ToVector2(), SpriteEffects.FlipVertically, 0);
                        //Console.WriteLine("Kollision bei X: " + collisionObject.X + " , Y: " + collisionObject.Y);
                        

                    }

                    if (b is Sprite)
                    {

                        Sprite gameSprite = b as Sprite;
                        //spriteBatch.Draw(Cichorium.Missing, b.Position, null, Color.Salmon, b.Rotation, Vector2.Zero, new Vector2(gameSprite.BodyWidth, gameSprite.BodyHeight) / Cichorium.Missing.Bounds.Size.ToVector2(), SpriteEffects.FlipVertically, 0);
                        //Console.WriteLine("GameSprite bei X: " + gameSprite.Position.X + " , Y: " + gameSprite.Position.Y);
                        if (gameSprite is Player)
                        {
                            Player p = gameSprite as Player;
                            
                            
                            //spriteBatch.Draw(Cichorium.Missing, p.secondBody.Position, null, Color.Salmon, p.secondBody.Rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        }

                    }

                    if (b is Portal)
                    {

                        Portal portal = b as Portal;
                        //spriteBatch.Draw(Cichorium.Missing, b.WorldCenter, null, Color.Yellow, b.Rotation, Vector2.Zero, new Vector2(portal.BoundingBox.Width, portal.BoundingBox.Height) / Cichorium.Missing.Bounds.Size.ToVector2(), SpriteEffects.None, 0);
                        //Console.WriteLine("Portal bei X: " + portal.Position.X + " , Y: " + portal.Position.Y);

                    }

                    if (b is ParticleEmitter)
                    {

                        ParticleEmitter emitter = b as ParticleEmitter;
                        emitter.Draw(spriteBatch);
                        //spriteBatch.Draw(Cichorium.Missing, b.Position, null, Color.Salmon, b.Rotation, Vector2.Zero, new Vector2(gameSprite.BodyWidth, gameSprite.BodyHeight) / Cichorium.Missing.Bounds.Size.ToVector2(), SpriteEffects.FlipVertically, 0);
                        //Console.WriteLine("GameSprite bei X: " + gameSprite.Position.X + " , Y: " + gameSprite.Position.Y);

                    }

                }
                spriteBatch.End();

            }
            base.Draw(gameTime);
        }

        public void reset()
        {
            GraphicsDevice graphics = Cichorium.Instance.GraphicsDevice;
            Camera = new Camera(graphics.Viewport);
            Camera.Scale = (int)(1 * ((float)graphics.Viewport.Width / 240));
            Camera.Focus = player.WorldCenter;
        }

        public override void Update(GameTime gameTime)
        {

            if (!Enabled)
            {
                return;
            }

            if (Pause)
            {
                return;
            }


            Water.Update(gameTime);

            if (CurrentRegion.Width > 10 && CurrentRegion.Height > 7)
            {
                //Camera.Focus = player.WorldCenter;
                float X = MathHelper.Lerp(Camera.Focus.X, player.WorldCenter.X, 0.05f);
                float Y = MathHelper.Lerp(Camera.Focus.Y, player.WorldCenter.Y, 0.05f);

                Camera.Focus = new Vector2(X, Y);
                //Camera.Focus = player.WorldCenter;

                if (Camera.Focus.X - ((Camera.Viewport.Width) / Camera.Scale) / 2 < 0)
                {
                    Camera.Focus = new Vector2((Camera.Viewport.Width / Camera.Scale) / 2, Camera.Focus.Y);
                }
                if (Camera.Focus.Y - ((Camera.Viewport.Height) / Camera.Scale) / 2 < 0)
                {
                    Camera.Focus = new Vector2(Camera.Focus.X, (Camera.Viewport.Height / Camera.Scale) / 2);
                }
                if (Camera.Focus.X + ((Camera.Viewport.Width) / Camera.Scale) / 2 > CurrentRegion.Width * CurrentRegion.TileSize)
                {
                    Camera.Focus = new Vector2((CurrentRegion.Width * CurrentRegion.TileSize) - (Camera.Viewport.Width / Camera.Scale) / 2, Camera.Focus.Y);
                }
                if (Camera.Focus.Y + ((Camera.Viewport.Height) / Camera.Scale) / 2 > CurrentRegion.Height * CurrentRegion.TileSize)
                {
                    Camera.Focus = new Vector2(Camera.Focus.X, (CurrentRegion.Height * CurrentRegion.TileSize) - (Camera.Viewport.Height / Camera.Scale) / 2);
                }
            }
            else
            {
                int scale = Camera.Viewport.Width / 240;
                scale = scale * 2;
                Camera.Focus = new Vector2(Camera.Viewport.Width / scale, Camera.Viewport.Height / scale);

            }

            Camera.FocusFocus();

            base.Update(gameTime);
        }
    }
}
