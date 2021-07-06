using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cichorium.Objects
{
    public class CollisionObject : Sprite
    {

        private Vector2 Size;

        public CollisionObject(float x, float y, float w, float h, BodyType bodyType) : base("pixel", x + (w / 2), y + (h / 2))
        {
            CreateRectangle(w, h, 1, Vector2.Zero);
            Size = new Vector2(w, h);
            BodyType = bodyType;
            Invisible = true;
        }

        public CollisionObject(float x, float y, float w, float h, Category cat, BodyType bodyType) : base("pixel", x + (w / 2), y + (h / 2))
        {
            CreateRectangle(w, h, 1, Vector2.Zero);
            Size = new Vector2(w, h);
            BodyType = bodyType;
            SetCollisionCategories(cat);
            Invisible = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Invisible)
                spriteBatch.Draw(Texture, Position, new Rectangle(Point.Zero, Size.ToPoint()), Color.White, Rotation, Size / 2, Scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

    }
}
