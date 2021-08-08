using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TetrisV4
{
    //A class to write and read highscores and error messages.
    //This class is not yet implemented.
    class FileWriter
    {
        enum ErrorCodes
        {
            Success,
            DirectoryIsNullOrEmpty,
            UnableToCreateFiles,
            UnidentifiedError
        }

        private readonly static string sHighscoreFileName = @"highscore.txt";
        private readonly static string sErrorLog = @"errorlog.txt";
        private static string sFileDirectory; 

        public FileWriter()
        {

        }

        private static bool IsNullOrEmpty(string input)
        {
            return input.Length < 1 || input == null;
        }

        //Bugged, check later.
        public static int InitializeFiles()
        {
            sFileDirectory = Path.GetFullPath(@"tetris_files\");

            if (IsNullOrEmpty(sFileDirectory))
            {
                return (int)ErrorCodes.DirectoryIsNullOrEmpty;
            }

            if(!File.Exists(sFileDirectory + sErrorLog))
            {
                File.Create(sFileDirectory + sErrorLog);
            }

            if(!File.Exists(sFileDirectory + sHighscoreFileName))
            {
                File.Create(sFileDirectory + sHighscoreFileName);
            }

            if (!File.Exists(sFileDirectory + sHighscoreFileName) || !File.Exists(sFileDirectory + sErrorLog))
            {
                return (int)ErrorCodes.UnableToCreateFiles;
            }

            return (int)ErrorCodes.Success;
        }

        public static void WriteErrorToLogFile(int errorCode)
        {
            //If we were unable to create the error log file in 'FileWriter.InitializeFiles()', do not attempt to write something to it.
            if (!File.Exists(sFileDirectory + sErrorLog))
                return;

            using (StreamWriter sw = new StreamWriter(sFileDirectory + sErrorLog, true))
            {
                string logTime = DateTime.Now.ToString();
                switch(errorCode)
                {
                    case (int)ErrorCodes.DirectoryIsNullOrEmpty:
                        sw.WriteLine(logTime + ": When getting path to directory for TetrisV4, it returned null or empty.");
                        break;

                    case (int)ErrorCodes.UnableToCreateFiles:
                        sw.WriteLine(logTime + ": Found path to TetrisV4 directory, but files could not be created. Found path: " + sFileDirectory);
                        break;

                    default:
                        sw.WriteLine(logTime + ": Unidentified error occurred.");
                        break;
                }
            }

        }

    }
}
