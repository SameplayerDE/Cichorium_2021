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
    public class NPC : Sprite
    {

        public NPC(string Texture, float x, float y, float w, float h) : base(Texture, x + (w / 2), y + (h / 2))
        {

            BodyType = BodyType.Dynamic;

            CreateCircle(12f, 70f, new Vector2(0f, 6f));
            Depth = 5;
            FixedRotation = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.Draw(Cichorium.TextureManager.Sprites["pixel"], new Rectangle(Position.ToPoint(), new Point(12 * 2, 12 * 2)), null, Color.Red * 0.5f, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

    }
}
