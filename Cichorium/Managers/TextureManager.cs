using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cichorium.Managers
{
    public class TextureManager
    {

        public bool Loaded = false;
        public Dictionary<string, Texture2D> Sprites { get; private set; } = new Dictionary<string, Texture2D>();
        public Dictionary<string, SpriteFont> Fonts { get; private set; } = new Dictionary<string, SpriteFont>();

        public void LoadContent(ContentManager Content)
        {
            Sprites.Add("pixel", Content.Load<Texture2D>("pixel"));
            Sprites.Add("npc", Content.Load<Texture2D>("npc"));
            Sprites.Add("tiles", Content.Load<Texture2D>("trey"));
            Sprites.Add("player", Content.Load<Texture2D>("player_sheet"));
            Sprites.Add("frog", Content.Load<Texture2D>("frog"));
            Sprites.Add("smoke", Content.Load<Texture2D>("smoke"));
            Sprites.Add("fly", Content.Load<Texture2D>("fly"));
            Sprites.Add("cursor", Content.Load<Texture2D>("cursor"));
            Sprites.Add("inventory", Content.Load<Texture2D>("ui_inventory"));

            //Nine-Tile Section

            Sprites.Add("panel", Content.Load<Texture2D>("ui_panel"));

            for (int y = 0; y < Sprites["panel"].Height / 7; y++)
            {
                for (int x = 0; x < Sprites["panel"].Width / 7; x++)
                {
                    Texture2D cropTexture = new Texture2D(Cichorium.Instance.GraphicsDevice, 7, 7);
                    Color[] data = new Color[7 * 7];
                    Sprites["panel"].GetData(0, new Rectangle(x * 7, y * 7, 7, 7), data, 0, data.Length);
                    cropTexture.SetData(data);
                    //Console.WriteLine((y * 3 + x));
                    Sprites.Add("panel-" + (y * 3 + x), cropTexture);
                }
            }

            Sprites.Add("button", Content.Load<Texture2D>("ui_button"));

            for (int y = 0; y < Sprites["button"].Height / 7; y++)
            {
                for (int x = 0; x < Sprites["button"].Width / 7; x++)
                {
                    Texture2D cropTexture = new Texture2D(Cichorium.Instance.GraphicsDevice, 7, 7);
                    Color[] data = new Color[7 * 7];
                    Sprites["button"].GetData(0, new Rectangle(x * 7, y * 7, 7, 7), data, 0, data.Length);
                    cropTexture.SetData(data);
                    //Console.WriteLine((y * 3 + x));
                    Sprites.Add("button-" + (y * 3 + x), cropTexture);
                }
            }

            Sprites.Add("textbox", Content.Load<Texture2D>("ui_textbox"));

            for (int y = 0; y < Sprites["textbox"].Height / 5; y++)
            {
                for (int x = 0; x < Sprites["textbox"].Width / 5; x++)
                {
                    Texture2D cropTexture = new Texture2D(Cichorium.Instance.GraphicsDevice, 5, 5);
                    Color[] data = new Color[5 * 5];
                    Sprites["textbox"].GetData(0, new Rectangle(x * 5, y * 5, 5, 5), data, 0, data.Length);
                    cropTexture.SetData(data);
                    //Console.WriteLine((y * 3 + x));
                    Sprites.Add("textbox-" + (y * 3 + x), cropTexture);
                }
            }

            Sprites.Add("textbox2", Content.Load<Texture2D>("ui_textbox_2"));

            for (int y = 0; y < Sprites["textbox2"].Height / 7; y++)
            {
                for (int x = 0; x < Sprites["textbox2"].Width / 7; x++)
                {
                    Texture2D cropTexture = new Texture2D(Cichorium.Instance.GraphicsDevice, 7, 7);
                    Color[] data = new Color[7 * 7];
                    Sprites["textbox2"].GetData(0, new Rectangle(x * 7, y * 7, 7, 7), data, 0, data.Length);
                    cropTexture.SetData(data);
                    //Console.WriteLine((y * 3 + x));
                    Sprites.Add("textbox2-" + (y * 3 + x), cropTexture);
                }
            }

            Sprites.Add("skillbadge", Content.Load<Texture2D>("skillbadge"));

            Sprites.Add("skillicon_bag", Content.Load<Texture2D>("skillicon_bag"));
            Sprites.Add("skillicon_star", Content.Load<Texture2D>("skillicon_star"));
            Sprites.Add("skillicon_shield", Content.Load<Texture2D>("skillicon_shield"));
            Sprites.Add("skillicon_heart", Content.Load<Texture2D>("skillicon_heart"));
            Sprites.Add("skillicon_locked", Content.Load<Texture2D>("skillicon_locked"));


            Fonts.Add("text", Content.Load<SpriteFont>("text"));
            Fonts.Add("default", Content.Load<SpriteFont>("default"));

            Loaded = true;
        }

    }
}
