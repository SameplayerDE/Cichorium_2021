using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using Cichorium.Managers;
using Cichorium;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Collision;

namespace Cichorium.Objects
{
    public class Frog : Sprite
    {

        public TimeSpan JumpUpdateRate = new TimeSpan(0, 0, 0, 2, 500);
        public TimeSpan SoundUpdateRate = new TimeSpan(0, 0, 0, 5, 500);
        public static readonly Random random = new Random();

        private TimeSpan JumpLastUpdate;
        private TimeSpan SoundLastUpdate;
        private bool big = false;
        private bool sound = false;
        private Vector2 Size;

        float saturation = 1;
        float distance = 1f;
        float currentDistance = 0f;
        float rotationZunge = 0f;
        bool visibleZunge = false;

        bool Caught = false;
        bool FullyOut = false;

        private readonly Vector2 Start;

        FlyParticle FlyTarget = null;

        public Frog(float x, float y, float w, float h) : base("frog", x + (w / 2), y + (h / 2))
        {
            BodyType = BodyType.Dynamic;

            //CreateRectangle(8f, 6f, 1f, Vector2.Zero);
            Size = new Vector2(w, h);
            CreateCircle(4f, 1f);
            Depth = 1;
            FixedRotation = true;
            JumpLastUpdate = new TimeSpan(0, 0, 0, random.Next(15, 30), 0);
            SoundLastUpdate = new TimeSpan(0, 0, 0, random.Next(0, 15), 0);
            saturation = random.Next(0, 2);
            Start = new Vector2(x + (w / 2), y + (h / 2));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            if (visibleZunge) {

                Point zunge = Position.ToPoint();
                zunge -= new Point(1, 0);
                spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], new Rectangle(zunge, new Point(2, (int)(currentDistance))), null, Color.Pink, rotationZunge + MathHelper.ToRadians(270), Vector2.Zero, SpriteEffects.None, 0);

            }

        }

        public override void Update(GameTime gameTime)
        {

            if (gameTime.TotalGameTime - JumpLastUpdate > JumpUpdateRate)
            {
                if (Vector2.Distance(Position, Start) > 50)
                {
                    ApplyLinearImpulse(-(Position - Start) * 100);
                }
                else {
                    ApplyLinearImpulse(new Vector2(random.Next(-8000, 8000), random.Next(-8000, 8000)));
                }
                
                JumpLastUpdate = gameTime.TotalGameTime;

            }

            if (saturation <= 0 && FlyTarget == null)
            {
                foreach (Body body in World.BodyList)
                {
                    if (body is FlyParticle)
                    {
                        FlyParticle fly = body as FlyParticle;

                        float distance = Vector2.Distance(fly.Position, Position);

                        if (distance < 25)
                        {
                            this.distance = distance;
                            FlyTarget = fly;
                            rotationZunge = (float)(Math.Atan2(FlyTarget.Position.Y - Position.Y, FlyTarget.Position.X - Position.X));
                        }
                    }
                }
            }

            

            if (sound)
            {
                
                if (big)
                    Scale -= 0.05f;

                if (!big)
                    Scale += 0.05f;

                if (Scale >= 1.5f)
                {
                    big = true;
                }

                if (Scale <= 1f)
                {
                    sound = !sound;
                    big = false;
                }

            }

            if (gameTime.TotalGameTime - SoundLastUpdate > SoundUpdateRate)
            {

                sound = !sound;

                float volume = 0.1f;
                float pitch = 0.0f;
                float pan = 0.0f;

                //volume /= Vector2.Distance(SceneManager.Camera.Focus, Position) / 100;

                Cichorium.AudioManager.SoundEffects["frog"].Play(volume, pitch, pan);

                SoundLastUpdate = gameTime.TotalGameTime;

            }

            saturation -= 0.001f;

            if (visibleZunge == false && saturation < 0f)
            {
                visibleZunge = true;
            }

            if (FullyOut && !Caught)
            {
                if (0 < currentDistance)
                {
                    currentDistance -= (distance / 20);
                }
                else
                {
                    visibleZunge = false;
                    distance = 0;
                    currentDistance = 0;
                    FlyTarget = null;
                    FullyOut = false;
                }
            }

            if (distance != 0 && FlyTarget != null && !(FullyOut && !Caught))
            {

                Vector2 Sticky = new Vector2((float)(currentDistance * Math.Cos(rotationZunge)) + Position.X, (float)(currentDistance * Math.Sin(rotationZunge)) + Position.Y);

                if (FlyTarget.Life <= 0)
                {
                    FullyOut = true;
                }
                else
                {

                    if (distance > currentDistance && !Caught)
                    {
                        currentDistance += (distance / 10);
                        //rotationZunge = (float)(Math.Atan2(FlyTarget.Position.Y - Position.Y, FlyTarget.Position.X - Position.X)) + MathHelper.ToRadians(270);
                    }
                    else
                    {
                        if (Vector2.Distance(FlyTarget.Position, Sticky) < 5)
                        {
                            Caught = true;
                        }
                        FullyOut = true;
                    }

                    if (Caught)
                    {
                        if (0 < currentDistance)
                        {
                            currentDistance -= (distance / 10);
                            FlyTarget.Position = Sticky;
                        }
                        else
                        {
                            visibleZunge = false;
                            FlyTarget.Life = 0f;
                            distance = 0;
                            currentDistance = 0;
                            saturation = 1f;
                            FlyTarget = null;
                            Caught = false;
                            FullyOut = false;
                        }
                    }
                }
            }

            
            
            base.Update(gameTime);
        }

    }
}
