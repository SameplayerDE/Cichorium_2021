using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace Cichorium.Objects
{
    public class FlyParticle : Particle
    {

        public static readonly Random random = new Random();
        int speed = 500;

        public FlyParticle(float x, float y) : base("fly", x, y)
        {
            //Mass = 100;
            Color = Color.White;
            Start = new Vector2(x, y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Position, Start) > 10)
            {
                ApplyLinearImpulse(-(Position - Start) * 100);
            }
            else {
                ApplyLinearImpulse(new Vector2(random.Next(-speed, speed), random.Next(-speed, speed)));
            }
            base.Update(gameTime);
        }

    }
}
