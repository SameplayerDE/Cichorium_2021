using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cichorium.Managers;
using Cichorium.Objects.ScreenObjects;
using Microsoft.Xna.Framework.Media;

namespace Cichorium.Objects.Screens
{
    public class ChangeLogScreen : Screen
    {

        public Container Container;

        public Label Heading;
        public TextArea Content;
        public Button Close;
        private Textbox box;

        public ChangeLogScreen() : base(4)
        {
            Heading = new Label(0, 0, "Change-Log");
            Content = new TextArea(0, 20, "Viele neue Sachen", "Hallo");
            Content.Scale = 0.5f;
            Close = new Button(0, 100, "K");

            box = new Textbox(0, 0, 100, 100, true, true, true);
            

            Container = new Container(0, 0, 100, 100, true, true);

            Container.Add(Heading);
            Container.Add(Content);
            Container.Add(Close);

            Add(Container);
            //Add(box);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Close.Clicked)
            {
                Cichorium.Instance.ScreenManager.CloseScreen();
                MediaPlayer.Resume();
                Cichorium.SimulationManager.Enabled = true;
                Cichorium.Instance.SceneManager.Pause = false;
                Cichorium.Instance.HudManager.Visible = true;
            }

        }

    }
}
