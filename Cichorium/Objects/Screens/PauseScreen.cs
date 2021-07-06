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
    public class PauseScreen : Screen
    {

        public Button Continue;
        public Button Exit;
        public Button Save;
        public Button Online;
        public Container Container;

        public PauseScreen() : base(2)
        {

            Continue = new Button(0, 0, "Weiter");
            Exit = new Button(0, 63, "Beenden");
            Online = new Button(0, 21, "Online");
            Save = new Button(0, 42, "Speichern");
            Container = new Container(0, 0, 100, 2 + 20 * 4, true, true);

            Container.Add(Continue);
            Container.Add(Online);
            Container.Add(Save);
            Container.Add(Exit);

            Add(Container);

        }

        public override void Update(GameTime gameTime)
        {
            if (Online.Clicked)
            {



            }

            base.Update(gameTime);
        }

    }
}
