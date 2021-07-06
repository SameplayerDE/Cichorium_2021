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
    public class SmokeParticle : Particle
    {

        public SmokeParticle(float x, float y) : base("smoke", x, y)
        {
            Scale = 1;
            Color = Color.DarkGray;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            ApplyLinearImpulse(new Vector2(-0.01f, -0.2f));
            Life -= 0.005f;
            Scale -= 0.0025f;
            Alpha -= 0.005f;
            base.Update(gameTime);
        }

    }
}
