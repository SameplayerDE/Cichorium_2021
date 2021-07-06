using Cichorium.Managers;
using Cichorium.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;
using Cichorium.Objects.Screens;
using System.Net;
using System.IO;
using Lidgren.Network;

namespace Cichorium
{
    public class Cichorium : Game
    {

        public static Cichorium Instance;
        public static readonly string Version = "1.7.4.4";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public int Resolution = 4;
        public static Texture2D Missing;
        public static Point Middle;
        public static Point Scale;

        public static TextureManager TextureManager { get; private set; } = new TextureManager();
        public static AudioManager AudioManager { get; private set; } = new AudioManager();
        public static SimulationManager SimulationManager { get; private set; } = new SimulationManager();
        public static InputManager InputManager { get; private set; } = new InputManager();

        public ScreenManager ScreenManager { get; private set; }
        public HudManager HudManager { get; private set; }
        public SceneManager SceneManager { get; private set; }

        public static SkillTreeScreen Skills;


        public enum Actions
        {
            Enter,
            Down,
            Left,
            Right,
            Up,
            Escape,
            Inventory,
            E,
            Q,
            Skills,
            LeftClicked,
            MenuDown,
            MenuUp,
            MenuYes,
            MenuNo
        }

        public Cichorium()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Instance = this;

            graphics.PreferredBackBufferHeight = 168 * Resolution;
            graphics.PreferredBackBufferWidth = 240 * Resolution;

            Middle = new Point(240 / 2, 168 / 2);
            Scale = new Point(240, 168);

            //Window.IsBorderless = true;
            Window.AllowUserResizing = true;
            Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));

            IsMouseVisible = true;

            this.IsFixedTimeStep = false;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 120d); //60);

            graphics.ApplyChanges();

            Missing = Content.Load<Texture2D>("Missing");

            var inputAction = new InputAction((int)Actions.Escape, TriggerState.Pressed)
            {
                GamePadButton = Buttons.Start,
                KeyButton = Keys.Escape
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.LeftClicked, TriggerState.Pressed)
            {
                MouseButton = MouseButtons.LeftButton
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Down, TriggerState.Down)
            {
                GamePadButton = Buttons.DPadDown,
                KeyButton = Keys.S
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Skills, TriggerState.Pressed)
            {
                GamePadButton = Buttons.Back,
                KeyButton = Keys.X
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Inventory, TriggerState.Pressed)
            {
                GamePadButton = Buttons.Y,
                KeyButton = Keys.I
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.E, TriggerState.Pressed)
            {
                GamePadButton = Buttons.RightShoulder,
                KeyButton = Keys.E
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Q, TriggerState.Pressed)
            {
                GamePadButton = Buttons.LeftShoulder,
                KeyButton = Keys.Q
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Right, TriggerState.Down)
            {
                GamePadButton = Buttons.DPadRight,
                KeyButton = Keys.D
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Up, TriggerState.Down)
            {
                GamePadButton = Buttons.DPadUp,
                KeyButton = Keys.W
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.MenuDown, TriggerState.Pressed)
            {
                GamePadButton = Buttons.DPadDown
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.MenuUp, TriggerState.Pressed)
            {
                GamePadButton = Buttons.DPadUp
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.MenuYes, TriggerState.Pressed)
            {
                GamePadButton = Buttons.A
            };

            InputManager.MapAction(inputAction);

            inputAction = new InputAction((int)Actions.Left, TriggerState.Down)
            {
                GamePadButton = Buttons.DPadLeft,
                KeyButton = Keys.A
            };

            InputManager.MapAction(inputAction);

            SceneManager = new SceneManager(this);
            SceneManager.DrawOrder = 1;
            SceneManager.Enabled = false;
            Components.Add(SceneManager);

            HudManager = new HudManager(this);
            HudManager.DrawOrder = 2;
            HudManager.Enabled = false;
            Components.Add(HudManager);

            ScreenManager = new ScreenManager(this);
            ScreenManager.DrawOrder = 3;
            ScreenManager.Enabled = false;
            Components.Add(ScreenManager);

            SaveFileManager.Load();

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.LoadContent(Content);
            AudioManager.LoadContent(Content);

            if (TextureManager.Loaded && AudioManager.Loaded)
            {
                SceneManager.Enabled = true;
                HudManager.Enabled = true;
                ScreenManager.Enabled = true;
                InitGame();
            }

            
            // TODO: use this.Content to load your game content here

        }

        public void InitGame()
        {
            if (SaveFileManager.Data.Region != null)
                SceneManager.SetRegion(RegionLoader.LoadAll().First(a => a.Title.Equals(SaveFileManager.Data.Region)));
            else
                SceneManager.SetRegion(RegionLoader.LoadAll()[0]);
            SimulationManager.world = SimulationManager.CurrentRegion.World;
            Skills = new SkillTreeScreen();

            MediaPlayer.Play(AudioManager.Songs["song_s"]);
            MediaPlayer.Pause();
            
            MediaPlayer.Volume = 0.1f;

            var webRequest = WebRequest.Create(@"https://ignite-flyff.com/bigyikes.txt");

            try
            {
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent = reader.ReadToEnd();

                    if (strContent != Version)
                    {
                        ScreenManager.ShowScreen(new ChangeLogScreen());
                        SimulationManager.Enabled = false;
                        SceneManager.Pause = true;
                        HudManager.Visible = false;
                    }
                    else
                    {
                        MediaPlayer.Resume();
                    }

                }
            }catch(Exception e)
            {
                //No Connection possible
                MediaPlayer.Resume();
            }

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (!IsActive)
            {
                return;
            }

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(AudioManager.Songs["song_l"]);
                MediaPlayer.IsRepeating = true;
            }

            if (SimulationManager.Portal != null)
            {
                SceneManager.SetRegion(RegionLoader.LoadAll().First(a => a.Title.Equals(SimulationManager.Portal.Destination)));
                if (SimulationManager.Portal.Destination == "A")
                {
                    SaveFileManager.Save(true);
                }
                if (SimulationManager.CurrentRegion.Title == "C")
                {
                    SaveFileManager.Data.SkillTree.Skills[0].Unlock();
                }
                SimulationManager.Portal = null;
                SimulationManager.world = SimulationManager.CurrentRegion.World;
            }

            InputManager.Update();
            SimulationManager.Update(gameTime);

            if (ScreenManager.GetCurrentScreen != null)
            {
                if (ScreenManager.GetCurrentScreen.ID == 2)
                {
                    PauseScreen screen = ScreenManager.GetCurrentScreen as PauseScreen;
                    if (screen.Exit.Clicked)
                    {
                        if (SimulationManager.CurrentRegion.Title == "A")
                        {
                            SaveFileManager.Save(true);
                        }
                        
                        Exit();
                    }
                    if (screen.Save.Clicked)
                    {
                        SaveFileManager.Save(true);
                        ScreenManager.CloseScreen();
                        SimulationManager.Enabled = true;
                        SceneManager.Pause = false;
                    }
                    if (screen.Continue.Clicked)
                    {
                        ScreenManager.CloseScreen();
                        SimulationManager.Enabled = true;
                        SceneManager.Pause = false;
                    }
                }
            }

            if (InputManager.IsActionTriggered((int)Actions.E))
            {
                Resolution += 1;
                if (Resolution > 5)
                {
                    Resolution = 5;
                }
                graphics.PreferredBackBufferHeight = 168 * Resolution;
                graphics.PreferredBackBufferWidth = 240 * Resolution;
                
                Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));
                graphics.ApplyChanges();
                SceneManager.reset();
            }

            if (InputManager.IsActionTriggered((int)Actions.Q))
            {
                Resolution -= 1;
                if (Resolution <= 0)
                {
                    Resolution = 1;
                }
                graphics.PreferredBackBufferHeight = 168 * Resolution;
                graphics.PreferredBackBufferWidth = 240 * Resolution;
                Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));
                graphics.ApplyChanges();
                SceneManager.reset();
            }

            

            if (InputManager.IsActionTriggered((int)Actions.Escape))
            {
                if (ScreenManager.GetCurrentScreen != null)
                {
                    if (ScreenManager.GetCurrentScreen.ID == 4)
                    {
                        return;
                    }
                    if (ScreenManager.GetCurrentScreen.ID == 2)
                    {
                        ScreenManager.CloseScreen();
                        SimulationManager.Enabled = true;
                        SceneManager.Pause = false;
                    }
                    else
                    {
                        ScreenManager.CloseScreen();
                        //ScreenManager.ShowScreen(new PauseScreen());
                        SimulationManager.Enabled = true;
                        SceneManager.Pause = false;
                    }
                }
                else
                {
                    ScreenManager.ShowScreen(new PauseScreen());
                    SimulationManager.Enabled = false;
                    SceneManager.Pause = true;
                }
                //Exit();
            }

            if (InputManager.IsActionTriggered((int)Actions.Skills))
            {
                if (ScreenManager.GetCurrentScreen != null)
                {
                    if (ScreenManager.GetCurrentScreen.ID == 4)
                    {
                        return;
                    }
                    if (ScreenManager.GetCurrentScreen.ID == 1)
                    {
                        ScreenManager.CloseScreen();
                        SimulationManager.Enabled = true;
                        SceneManager.Pause = false;
                    }
                    else
                    {
                        ScreenManager.CloseScreen();
                        Skills.UpdateSkillTree();
                        ScreenManager.ShowScreen(Skills);
                        SimulationManager.Enabled = false;
                        SceneManager.Pause = true;
                    }
                }
                else
                {
                    Skills.UpdateSkillTree();
                    ScreenManager.ShowScreen(Skills);
                    SimulationManager.Enabled = false;
                    SceneManager.Pause = true;
                }
            }

            if (InputManager.IsActionTriggered((int)Actions.Inventory))
            {
                if (ScreenManager.GetCurrentScreen != null)
                {
                    if (ScreenManager.GetCurrentScreen.ID == 4)
                    {
                        return;
                    }
                    if (ScreenManager.GetCurrentScreen.ID == 7)
                    {
                        ScreenManager.CloseScreen();
                        SimulationManager.Enabled = true;
                        SceneManager.Pause = false;
                    }
                    else
                    {
                        ScreenManager.CloseScreen();
                        ScreenManager.ShowScreen(new InventoryScreen());
                        SimulationManager.Enabled = false;
                        SceneManager.Pause = true;
                    }
                }
                else
                {
                    ScreenManager.ShowScreen(new InventoryScreen());
                    SimulationManager.Enabled = false;
                    SceneManager.Pause = true;
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);

        }

        public static Point GetMiddle(int Width, int Height)
        {
            return Middle - (new Vector2(Width, Height) / 2).ToPoint();
        }
    }
}
