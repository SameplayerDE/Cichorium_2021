using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cichorium.Objects
{
    public class Camera
    {

        public Vector2 Focus = Vector2.Zero;
        public Matrix Transform { get; private set; }
        public Viewport Viewport { get; set; }
        public float Rotation = 0f;
        public float Scale { get; set; }

        public Camera(Viewport viewport)
        {
            Viewport = viewport;
            Scale = 1f;
        }

        public void SetFocus(Point target)
        {
            var position = Matrix.CreateTranslation(-target.X, -target.Y, 0);
            var offset = Matrix.CreateTranslation(Viewport.Width / 2, Viewport.Height / 2, 0);

            Transform = position * Matrix.CreateScale(Scale) * offset;
        }

        public void SetFocus(Vector2 target)
        {
            //Point t = target.ToPoint();
            var position = Matrix.CreateTranslation(-target.X, -target.Y, 0);
            var offset = Matrix.CreateTranslation(Viewport.Width / 2, Viewport.Height / 2, 0);

            Transform = position * Matrix.CreateScale(Scale) * offset;
        }

        public void FocusFocus()
        {
            Point f = Focus.ToPoint();
            var position = Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0);
            //var position = Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0);
            var offset = Matrix.CreateTranslation(Viewport.Width / 2, Viewport.Height / 2, 0);

            Transform = position * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) * Matrix.CreateScale(Scale) * offset;
        }

    }
}
