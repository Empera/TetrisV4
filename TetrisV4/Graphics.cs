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
            "Back"
        };
        static readonly float PointerPosition = 500.0f;
        static readonly float OptionsPointerPosition = 300.0f;

        static readonly float fSideTextXPosition = 380.0f;
        static readonly float fSideTextYPosition = 220.0f;

        static readonly float fOptionsTextXPosition = 300.0f;
        static readonly float fOptionsTextYPosition = 250.0f;

        static readonly int iTextOffset = 30;

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
                MessageBox.Show("Error: " + e.ToString());
                block_images = null;
                return false;

            }

            return true;
        }

        static public void DrawMainMenu(PaintEventArgs e, int iMainMenuCursorPosition)
        {
            //Just to make the background black.
            e.Graphics.FillRectangle(cBlack, background);

            e.Graphics.DrawString("Tetris", fnt_game, cAzure, 270.0f, 200.0f);
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


            //Print the Tetris map.
            for (int y = 0; y < 22; ++y)
                for (int x = 0; x < 21; ++x)
                {
                    fieldPoint.X = (x * 30);
                    fieldPoint.Y = (y * 30);

                    gameFieldBackground.X = fieldPoint.X;
                    gameFieldBackground.Y = fieldPoint.Y;

                    switch (tetrisMap[x, y])
                    {
                        case BLOCK.EMPTY:   e.Graphics.FillRectangle(cBlack, gameFieldBackground);                      break;
                        case BLOCK.BORDER:  e.Graphics.DrawImage(block_images[(int)BLOCK.BORDER - 1], fieldPoint);      break;
                        case BLOCK.YELLOW:  e.Graphics.DrawImage(block_images[(int)BLOCK.YELLOW - 1], fieldPoint);      break;
                        case BLOCK.BLUE:    e.Graphics.DrawImage(block_images[(int)BLOCK.BLUE - 1], fieldPoint);        break;
                        case BLOCK.GREEN:   e.Graphics.DrawImage(block_images[(int)BLOCK.GREEN - 1], fieldPoint);       break;
                        case BLOCK.PURPLE:  e.Graphics.DrawImage(block_images[(int)BLOCK.PURPLE - 1], fieldPoint);      break;
                        case BLOCK.RED:     e.Graphics.DrawImage(block_images[(int)BLOCK.RED - 1], fieldPoint);         break;
                        case BLOCK.ORANGE:  e.Graphics.DrawImage(block_images[(int)BLOCK.ORANGE - 1], fieldPoint);      break;
                        case BLOCK.CYAN:    e.Graphics.DrawImage(block_images[(int)BLOCK.CYAN - 1], fieldPoint);        break;
                        default:    break;
                    }
                }

            //Print the active block.
            for (int x = 0; x < 4; ++x)
                for (int y = 0; y < 4; ++y)
                {
                    fieldPoint.X = ((x + posX) * 30);
                    fieldPoint.Y = ((y + posY) * 30);

                    switch (activeBlock[x, y])
                    {
                        case BLOCK.YELLOW:  e.Graphics.DrawImage(block_images[(int)BLOCK.YELLOW - 1], fieldPoint);      break;
                        case BLOCK.BLUE:    e.Graphics.DrawImage(block_images[(int)BLOCK.BLUE - 1], fieldPoint);        break;
                        case BLOCK.GREEN:   e.Graphics.DrawImage(block_images[(int)BLOCK.GREEN - 1], fieldPoint);       break;
                        case BLOCK.PURPLE:  e.Graphics.DrawImage(block_images[(int)BLOCK.PURPLE - 1], fieldPoint);      break;
                        case BLOCK.RED:     e.Graphics.DrawImage(block_images[(int)BLOCK.RED - 1], fieldPoint);         break;
                        case BLOCK.ORANGE:  e.Graphics.DrawImage(block_images[(int)BLOCK.ORANGE - 1], fieldPoint);      break;
                        case BLOCK.CYAN:    e.Graphics.DrawImage(block_images[(int)BLOCK.CYAN - 1], fieldPoint);        break;
                        default:    break;
                    }
                }

            //Print the upcoming block.
            for (int x = 0; x < 4; ++x)
                for (int y = 0; y < 4; ++y)
                {
                    fieldPoint.X = ((x + 14) * 30);
                    fieldPoint.Y = ((y + 1) * 30);

                    switch (nextBlock[x, y])
                    {
                        case BLOCK.YELLOW:  e.Graphics.DrawImage(block_images[(int)BLOCK.YELLOW - 1], fieldPoint);      break;
                        case BLOCK.BLUE:    e.Graphics.DrawImage(block_images[(int)BLOCK.BLUE - 1], fieldPoint);        break;
                        case BLOCK.GREEN:   e.Graphics.DrawImage(block_images[(int)BLOCK.GREEN - 1], fieldPoint);       break;
                        case BLOCK.PURPLE:  e.Graphics.DrawImage(block_images[(int)BLOCK.PURPLE - 1], fieldPoint);      break;
                        case BLOCK.RED:     e.Graphics.DrawImage(block_images[(int)BLOCK.RED - 1], fieldPoint);         break;
                        case BLOCK.ORANGE:  e.Graphics.DrawImage(block_images[(int)BLOCK.ORANGE - 1], fieldPoint);      break;
                        case BLOCK.CYAN:    e.Graphics.DrawImage(block_images[(int)BLOCK.CYAN - 1], fieldPoint);        break;
                        default:    break;
                    }
                }

            e.Graphics.DrawString(sideText[0] + highscore, fnt, cAzure, fSideTextXPosition, fSideTextYPosition);
            e.Graphics.DrawString(sideText[1], fnt, cAzure, fSideTextXPosition, fSideTextYPosition + iTextOffset * 3);
            e.Graphics.DrawString(sideText[2], fnt, cAzure, fSideTextXPosition, fSideTextYPosition + iTextOffset * 4);

            if(gameOver)
            {
                e.Graphics.DrawString("Game over.\n<Enter> - Restart.", fnt, cAzure, fSideTextXPosition, fSideTextYPosition + iTextOffset * 6);
            }

        }

        static public void DrawOptionsMenu(PaintEventArgs e, int iOptionsCursorPosition)
        {
            e.Graphics.FillRectangle(cBlack, background);

            e.Graphics.DrawString(optionsText[0] + (Sound.IsEnabled() ? "Enabled" : "Disabled"), fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 50);
            e.Graphics.DrawString(optionsText[1] + Sound.GetVolumeLevel() + "%", fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 100);
            e.Graphics.DrawString(optionsText[2] + "1x", fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 150);
            e.Graphics.DrawString(optionsText[3], fnt, cAzure, fOptionsTextXPosition, fOptionsTextYPosition + 200);

            e.Graphics.DrawString(PointerForCursorPosition, fnt, cAzure, fOptionsTextXPosition - 50.0f, OptionsPointerPosition + 50.0f * iOptionsCursorPosition);
        }
    }
}
