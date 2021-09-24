using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TetrisV4
{
    class Graphics
    {
        Form f;

        static Rectangle background;
        static RectangleF gameFieldBackground = new RectangleF(0, 0, 30, 30);
        static SolidBrush cBlack = new SolidBrush(Color.Black);
        static SolidBrush cAzure = new SolidBrush(Color.FloralWhite);
        static Size blockSize = new Size(30, 30);

        static Font fnt = new Font("Comic Sans MS", 18.0f);
        static Font fnt_game = new Font("Trebuchet MS", 60.0f);

        static public bool bDisplayKeybindsInOptionsMenu = false;

        static string PointerForCursorPosition = "->";
        static string[] sideText =
        {
            "Highscore: ",
            "<Esc> Main menu.",
            "<P> Pause game."
        };
        public static string[] optionsText =
        {
            "Music: ",
            "Volume: ",
            "Speed: ",
            "Keybinds",
            "Back"
        };
        private static string[] keybinds =
        {
            "<M> - Mute/Unmute music",
            "<P> - Pause",
            "<R> - Rotate block",
            "<Left key> - Push block left",
            "<Right key> - Push block right",
            "<Down key> - Quickly move block downwards"
        };
        private static string[] tetris_logo =
        {
            "TTT EEE ZZZ RRR  III SSS",
            " T  E    Z  R  R  I  S    ",
            " T  EEE  Z  RRR   I  SSS ",
            " T  E    Z  R  R  I    S",
            " T  EEE  Z  R  R III SSS ",
        };

        static readonly float PointerPosition = 500.0f;
        static readonly float OptionsPointerPosition = 300.0f;

        static readonly float fSideTextXPosition = 380.0f;
        static readonly float fSideTextYPosition = 220.0f;

        static readonly int block_size = 30;
        static readonly int logo_block_size = 20;
        static readonly int nextBlockXPos = 14;
        static readonly int nextBlockYPos = 1;
        static readonly int logoXPos = 1;
        static readonly int logoYPos = 7;

        static readonly float fOptionsTextXPosition = 300.0f;
        static readonly float fOptionsTextYPosition = 250.0f;



        static readonly int iTextOffset = 50;

        static Image[] block_images;
        


        public Graphics(Form _f)
        {
            f = _f;
            background = new Rectangle(0, 0, f.Size.Width, f.Size.Height);
        }

        static public bool LoadGameFiles()
        {
            try
            {

                block_images = new Image[]
                {
                    Image.FromFile(@"tetris_images\blue.png"),
                    Image.FromFile(@"tetris_images\yellow.png"),
                    Image.FromFile(@"tetris_images\green.png"),
                    Image.FromFile(@"tetris_images\violet.png"),
                    Image.FromFile(@"tetris_images\red.png"),
                    Image.FromFile(@"tetris_images\orange.png"),
                    Image.FromFile(@"tetris_images\cyan.png"),
                    Image.FromFile(@"tetris_images\grey.png")

                };

            }
            catch (Exception e)
            {
                block_images = null;
                return false;

            }

            return true;
        }

        static public void DrawMainMenu(PaintEventArgs e, int iMainMenuCursorPosition)
        {
            //Just to make the background black.
            e.Graphics.FillRectangle(cBlack, background);

            DrawBlocks(e, ConvertStringToBlock(tetris_logo), logoXPos, logoYPos);

            //e.Graphics.DrawString("Tetris", fnt_game, cAzure, 270.0f, 200.0f);
            e.Graphics.DrawString("Play", fnt, cAzure, 350.0f, 500.0f);
            e.Graphics.DrawString("Highscores", fnt, cAzure, 350.0f, 550.0f);
            e.Graphics.DrawString("Options", fnt, cAzure, 350.0f, 600.0f);
            e.Graphics.DrawString("Quit", fnt, cAzure, 350.0f, 650.0f);
            
            e.Graphics.DrawString(PointerForCursorPosition, fnt, cAzure, 300.0f, PointerPosition + 50.0f * iMainMenuCursorPosition);
        }

        static public void DrawGame(PaintEventArgs  e, 
                                    Point           fieldPoint, 
                                    int             posX, 
                                    int             posY, 
                                    BLOCK[,]        tetrisMap, 
                                    BLOCK[,]        activeBlock, 
                                    BLOCK[,]        nextBlock, 
                                    int             highscore, 
                                    bool            gameOver)
        {
            e.Graphics.FillRectangle(cBlack, background);


            DrawBlocks(e, tetrisMap, 0, 0);
            DrawBlocks(e, activeBlock, posX, posY);
            DrawBlocks(e, nextBlock, nextBlockXPos, nextBlockYPos);

            e.Graphics.DrawString(sideText[0] + highscore, fnt, cAzure, fSideTextXPosition, fSideTextYPosition);
            e.Graphics.DrawString(sideText[1], fnt, cAzure, fSideTextXPosition, fSideTextYPosition + iTextOffset * 3);
            e.Graphics.DrawString(sideText[2], fnt, cAzure, fSideTextXPosition, fSideTextYPosition + iTextOffset * 4);
            
            if(gameOver)
            {
                e.Graphics.DrawString("Game over.\n<Enter> - Restart.", fnt, cAzure, fSideTextXPosition, fSideTextYPosition + iTextOffset * 6);
            }

        }

        static private void DrawBlocks(PaintEventArgs e, BLOCK[,] blocks, int Xpos, int Ypos)
        {

            for(int x = 0; x < blocks.GetLength(0); ++x) {
                for(int y = 0; y < blocks.GetLength(1); ++y) {
                    
                    switch(blocks[x, y])
                    {
                        case BLOCK.BORDER:  e.Graphics.DrawImage(block_images[(int)BLOCK.BORDER - 1],   (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.YELLOW:  e.Graphics.DrawImage(block_images[(int)BLOCK.YELLOW - 1],   (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.BLUE:    e.Graphics.DrawImage(block_images[(int)BLOCK.BLUE - 1],     (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.GREEN:   e.Graphics.DrawImage(block_images[(int)BLOCK.GREEN - 1],    (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.PURPLE:  e.Graphics.DrawImage(block_images[(int)BLOCK.PURPLE - 1],   (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.RED:     e.Graphics.DrawImage(block_images[(int)BLOCK.RED - 1],      (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.ORANGE:  e.Graphics.DrawImage(block_images[(int)BLOCK.ORANGE - 1],   (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        case BLOCK.CYAN:    e.Graphics.DrawImage(block_images[(int)BLOCK.CYAN - 1],     (Xpos + x) * block_size, (Ypos + y) * block_size);  break;
                        default: break;
                    }

                }
            }
        }

        static public void DrawOptionsMenu(PaintEventArgs e, int iOptionsCursorPosition)
        {
            e.Graphics.FillRectangle(cBlack, background);
            if (!bDisplayKeybindsInOptionsMenu)
            {
                

                e.Graphics.DrawString(optionsText[0] + (Sound.IsEnabled() ? "Enabled" : "Disabled"), fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 50);
                e.Graphics.DrawString(optionsText[1] + Sound.GetVolumeLevel() + "%", fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 100);
                e.Graphics.DrawString(optionsText[2] + "1x", fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 150);
                e.Graphics.DrawString(optionsText[3], fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 200);
                e.Graphics.DrawString(optionsText[4], fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 250);

                e.Graphics.DrawString(PointerForCursorPosition, fnt, cAzure, fOptionsTextXPosition - 50.0f, OptionsPointerPosition + 50.0f * iOptionsCursorPosition);
                return;
            }

            for(int x = 0; x < keybinds.Length; ++x)
            {
                e.Graphics.DrawString(keybinds[x], fnt, cAzure, fOptionsTextXPosition - 50.0f, fOptionsTextYPosition + iTextOffset * x);
            }

            e.Graphics.DrawString("Back", fnt, cAzure, fOptionsTextXPosition - 50.0f, fOptionsTextYPosition + iTextOffset * (keybinds.Length + 1));
            e.Graphics.DrawString(PointerForCursorPosition, fnt, cAzure, fOptionsTextXPosition - 100.0f, fOptionsTextYPosition + iTextOffset * (keybinds.Length + 1));
        }

        static public void DrawHighscore(PaintEventArgs e)
        {

        }

        static private BLOCK[,] ConvertStringToBlock(string[] array)
        {
            BLOCK[,] convertedBlock = new BLOCK[array[0].Length, array.Length];

            for(int x = 0; x < array.Length; ++x) { 
                for(int y = 0; y < array[0].Length; ++y)
                {
                    string temp = array[x];
                    
                    switch(temp[y])
                    {
                        case 'T':   convertedBlock[y, x] = BLOCK.BLUE;   break;
                        case 'E':   convertedBlock[y, x] = BLOCK.YELLOW; break;
                        case 'Z':   convertedBlock[y, x] = BLOCK.CYAN;   break;
                        case 'R':   convertedBlock[y, x] = BLOCK.GREEN;  break;
                        case 'I':   convertedBlock[y, x] = BLOCK.PURPLE; break;
                        case 'S':   convertedBlock[y, x] = BLOCK.RED;    break;
                        case ' ':   convertedBlock[y, x] = BLOCK.EMPTY;  break;
                        default:  break;
                    }
                }
            }
            return convertedBlock;
        }
    }
}
