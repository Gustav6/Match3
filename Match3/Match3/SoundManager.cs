using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class SoundManager
    {
        public static SoundEffect collectSound;
        public static Song backgroundMusic;

        public static void LoadSounds(ContentManager content)
        {
            collectSound = content.Load<SoundEffect>("collect");
            backgroundMusic = content.Load<Song>("backgroundMusic");
        }
    }
}
