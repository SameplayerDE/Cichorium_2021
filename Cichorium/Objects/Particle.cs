using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace Cichorium.Objects
{
    public class Particle : Sprite
    {
        public float Life { get; set; } = 1f;

        public bool AlphaEqualsLife = false;
        public bool Dead { set; protected get; } = false;
        public bool IsDead { get { return Dead; } }

        public Vector2 Start;

        public Particle(string assetFile, float x, float y) : base(assetFile, x, y)
        {
            Scale = 1f;
            FixedRotation = true;
            Depth = 5;
            BodyType = BodyType.Dynamic;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Dead)
                base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (AlphaEqualsLife)
            {
                Alpha = Life;
            }
            if (Life <= 0f)
            {
                Dead = true;
            }
            if (Dead && Life > 0f)
            {
                Dead = false;
                Alpha = 1f;
                ResetDynamics();
            }
        }
    }
}
