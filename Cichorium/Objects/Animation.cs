using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Cichorium
{
    public class Animation
    {

        public Texture2D Texture;
        public Texture2D[] Frames { get; private set; }
        public int Frame { get; private set; } = 0;
        public Point FrameSize { get; private set; }

        private TimeSpan AnimationDuration = new TimeSpan(0, 0, 0, 0, 250);
        private TimeSpan LastUpdate = new TimeSpan();
        private bool Loop = true;
        private bool Playing = false;

        public Animation(string File, int Width, int Height, TimeSpan AnimationDuration)
        {
            LoadTexture(File);
            FrameSize = new Point(Width, Height);
            Frames = new Texture2D[Texture.Height / FrameSize.Y];
            CutFrames();
            this.AnimationDuration = AnimationDuration;
            Playing = true;
        }

        private void CutFrames()
        {
            for (int i = 0; i < Texture.Height / FrameSize.Y; i++)
            {
                Texture2D cropTexture = new Texture2D(Cichorium.Instance.GraphicsDevice, FrameSize.X, FrameSize.Y);
                Color[] data = new Color[FrameSize.X * FrameSize.Y];
                Texture.GetData(0, new Rectangle(0, FrameSize.Y * i, FrameSize.X, FrameSize.Y), data, 0, data.Length);
                cropTexture.SetData(data);
                Frames[i] = cropTexture;
            }
        }

        private void LoadTexture(string File)
        {
            FileStream fileStream = new FileStream(File, FileMode.Open);
            Texture = Texture2D.FromStream(Cichorium.Instance.GraphicsDevice, fileStream);
            fileStream.Dispose();
        }

        public void Update(GameTime GameTime)
        {
            if (Playing)
            {
                if (GameTime.TotalGameTime - LastUpdate >= AnimationDuration)
                {
                    Frame++;
                    if (Frame >= Frames.Length)
                    {
                        Frame = 0;
                        if (!Loop)
                        {
                            Playing = false;
                        }
                    }
                    LastUpdate = GameTime.TotalGameTime;
                }
            }
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(Frames[Frame], Vector2.Zero, Color.White);
        }

    }
}

