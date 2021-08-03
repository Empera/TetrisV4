using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisV4
{
    class Sound
    {
        
        

        private static int volumeLevel = 100;
        public static bool EnableSound = true;

        public Sound()
        {
            
        }

        public static void PlayMusic()
        {

        }

        public static void StopMusic()
        {

        }

        public static bool IsEnabled()
        {
            return EnableSound;
        }

        //Bugged, troubleshoot further.
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
