using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cichorium.Managers;
using Cichorium.Objects.ScreenObjects;

namespace Cichorium.Objects.Screens
{
    public class TestScreen : DragableScreen
    {

        public TestScreen() : base(0)
        {
            DrawBackground = true;
            Position = new Point(0, 0);
            Size = Cichorium.Scale;
            Alpha = 0.9f;
            Color = Color.Black;
        }

    }
}
