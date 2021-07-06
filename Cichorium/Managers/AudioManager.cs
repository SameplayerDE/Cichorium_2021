using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cichorium.Managers
{
    public class AudioManager
    {

        public bool Loaded = false;
        public Dictionary<string, Song> Songs { get; private set; } = new Dictionary<string, Song>();
        public Dictionary<string, SoundEffect> SoundEffects { get; private set; } = new Dictionary<string, SoundEffect>();

        public void LoadContent(ContentManager Content)
        {
            SoundEffects.Add("frog", Content.Load<SoundEffect>("frog_sound"));
            SoundEffects.Add("button_interact", Content.Load<SoundEffect>("button_interact"));
            SoundEffects.Add("skill_unlocked", Content.Load<SoundEffect>("skill_unlocked"));
            SoundEffects.Add("skill_denied", Content.Load<SoundEffect>("skill_denied"));
            SoundEffects.Add("player_death", Content.Load<SoundEffect>("player_death"));
            SoundEffects.Add("region_change", Content.Load<SoundEffect>("region_change"));
            
            
            //Songs.Add("song", Content.Load<Song>("song"));
            Songs.Add("song_s", Content.Load<Song>("song_start"));
            Songs.Add("song_l", Content.Load<Song>("song_loop"));
            Loaded = true;
        }

    }
}

