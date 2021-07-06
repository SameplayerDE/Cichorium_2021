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
    public class TextboxScreen : Screen
    {

        public Textbox Box;

        public TextboxScreen(string message, int x, int y, int width, int height, bool calculatePadding, bool center, bool CenterAsCenter) : base(3)
        {

            Box = new Textbox(x, y, width, height, calculatePadding, center, CenterAsCenter);
            Box.Message = message;

            Add(Box);

        }



    }
}
