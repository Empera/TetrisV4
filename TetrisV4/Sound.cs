using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisV4
{
    class Sound
    {
        
        static System.Media.SoundPlayer music_library;
        private static string sMusicDirectory = System.IO.Path.GetFullPath(@"tetris_images\tetris_theme.wav");
        
        private static int volumeLevel = 100;
        private static bool EnableSound = true;
        private static bool bIsMusicPlaying = true;

        public Sound()
        {
            
        }

        public static void InitializeSound()
        {
            try
            {
                music_library = new System.Media.SoundPlayer(sMusicDirectory);
                PlayMusic();
            }
            catch(Exception e)
            {
                
            }
        }

       public static bool IsMusicPlaying()
        {
            return bIsMusicPlaying;
        }

        public static void Enable(bool enable)
        {
            if((EnableSound = enable) == true)
            {
                PlayMusic();
                return;
            }

            StopMusic();
        }

        private static void StopMusic()
        {
            music_library.Stop();
            bIsMusicPlaying = false;
        }

        private static void PlayMusic()
        {
            music_library.PlayLooping();
            bIsMusicPlaying = true;
        }

        public static bool IsEnabled()
        {
            return EnableSound;
        }

        //Bugged, troubleshoot further.
        //These two will not work with SoundPlayer.
        //Will try to implement WindowsMediaPlayer perhaps, which supports volume adjustments.
        public static void SetVolumeLevel(bool increase)
        {
            if(increase && volumeLevel < 100)
            {
                volumeLevel += 10;
                return;
            }
            else if (!increase && volumeLevel > 0)
            {
                volumeLevel -= 10;
                return;
            }
        }

        public static int GetVolumeLevel()
        {
            return volumeLevel;
        }

    }
}
