using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace Cichorium.Objects
{
    public class ParticleEmitter : Sprite
    {

        public Particle particle;
        public List<Particle> particles = new List<Particle>();
        public int Max = 100;
        public Rectangle Area;
        public TimeSpan UpdateRate = new TimeSpan(0, 0, 0, 0, 500);
        private static readonly Random random = new Random();
        public bool Friction = false;

        private TimeSpan LastUpdate = new TimeSpan(0, 0, 0, 0, 0);

        public ParticleEmitter(float x, float y, float w, float h, BodyType bodyType) : base("pixel", x + (w / 2), y + (h / 2))
        {
            Invisible = true;
        }

        public void SetParticle(Particle particle)
        {
            this.particle = particle;
            this.particle.Position = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
            {
                
                p.Draw(spriteBatch);

            }
            
        }

        public override void Update(GameTime gameTime)
        {

            if (gameTime.TotalGameTime - LastUpdate > UpdateRate)
            {

                if (particles.Count < Max)
                {
                    Particle p = (Particle)Activator.CreateInstance(particle.GetType(), random.Next(Area.Left, Area.Right), random.Next(Area.Top, Area.Bottom));
                    particles.Add(p);
                }

                foreach (Particle p in particles)
                {

                    if (!World.BodyList.Contains(p))
                    {
                        if (Friction)
                        {

                            FrictionJoint fj = new FrictionJoint(p, this, p.WorldCenter);
                            fj.MaxForce = 20000f;

                            World.Add(fj);

                        }
                        World.Add(p);
                    }

                    p.Update(gameTime);

                    if (p.IsDead)
                    {
                        Vector2 newPos = new Vector2(random.Next(Area.Left, Area.Right), random.Next(Area.Top, Area.Bottom));
                        p.Position = newPos;
                        p.Start = newPos;
                        p.Scale = 1f;
                        p.Life = 1f;
                    }

                }

                LastUpdate = gameTime.TotalGameTime;

            }

        }

        public float GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return (float) (random.NextDouble() * (maximum - minimum) + minimum);
        }

    }

}
