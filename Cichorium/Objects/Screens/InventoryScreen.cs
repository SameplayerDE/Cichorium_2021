using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cichorium.Managers;
using Cichorium.Objects.ScreenObjects;
using Lidgren.Network;

namespace Cichorium.Objects.Screens
{
    public class InventoryScreen : Screen
    {

        public Image Background;


        public InventoryScreen() : base(7)
        {

            Background = new Image(Cichorium.Middle.X, Cichorium.Middle.Y, "inventory");

            Add(Background);

        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

    }
}
