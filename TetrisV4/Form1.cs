using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisV4
{
    enum BLOCK
    {
        EMPTY,
        BLUE,
        YELLOW,
        GREEN,
        PURPLE,
        RED,
        ORANGE,
        CYAN,
        BORDER
    }

    

    public partial class tetris_main_form : Form
    {
        //*****************************************
        // I chose to rebuild Tetris a 4th time to simplify the logic further.
        // All logic will now be handled directly in the main class, while
        // the graphics will be handled through the Graphics class,
        // the sound through the Sound class, etc.
        //*****************************************

        #region Variables and objects
        static SizeF size = new SizeF(30, 30); // The blocks are 30 in size each.
        static Point fieldPoint = new Point();
        static Graphics g;

        static Random rand = new Random();

        private static int[][] tetromino = new int[][]
       {
            new int[] { 0, 0, 0, 0,
                        0, 1, 1, 0,
                        0, 1, 1, 0,
                        0, 0, 0, 0},

            new int[] { 0, 0, 0, 0,
                        0, 0, 1, 0,
                        0, 0, 1, 0,
                        0, 1, 1, 0},

            new int[] { 0, 0, 0, 0,
                        0, 0, 1, 0,
                        0, 1, 1, 0,
                        0, 1, 0, 0},

            new int[] { 0, 0, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 1, 0,
                        0, 0, 1, 0},

            new int[] { 0, 0, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 1, 0},

            new int[] { 0, 0, 0, 0,
                        0, 1, 0, 0,
                        0, 1, 1, 0,
                        0, 1, 0, 0},

            new int[] { 0, 0, 1, 0,
                        0, 0, 1, 0,
                        0, 0, 1, 0,
                        0, 0, 1, 0},
       };

        static private int posX = 0, posY = 0;
        static private int iMainMenuCursorPosition = 0, iOptionsCursorPosition = 0;
        static private int IWhatToDraw = 0;
        static private int iHighscore = 0;

        static private float fVersion = 0.2f;

        static private bool bGameOver = false;
        static private bool bGameIsRunning = true;
        static private bool bDisplayKeybinds = false;
        
        static BLOCK[,] tetrisMap = new BLOCK[21, 22];
        static BLOCK[,] activeBlock = new BLOCK[4, 4];
        static BLOCK[,] nextBlock = new BLOCK[4, 4];
        static BLOCK[,] nextBlockWindow = new BLOCK[6, 6];

        enum Direction
        {
            DOWN,
            RIGHT,
            LEFT
        }

        enum Draw
        {
            MainMenu,
            Game,
            Highscore,
            Options
        }


        #endregion
        public tetris_main_form()
        {
            InitializeComponent();

            KeyDown += Key_Down_Event_Handler;
            KeyUp += Key_Up_Event_Handler;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Width = 800;
            Height = 900;

            DoubleBuffered = true;
            draw_refresh_timer.Enabled = true;
            draw_refresh_timer.Start();
            game_tick_timer.Enabled = true;
            
            InitializeRequiredData();


        }

        //This method ensures all classes load their respective data.
        //If something fails, it will quit the application if it cannot run without it,
        //or just log an error message and go on if it's safe to continue.
        private void InitializeRequiredData()
        {
            g = new Graphics(this);

            if(!Graphics.LoadGameFiles()) //If the files are not loaded, exit the application.
            {
                MessageBox.Show("Required game files could not be loaded due to being missing or corrupted. Program will exit.");
                FileWriter.WriteErrorToLogFile(0);
                Environment.Exit(1);
            }

            int status = FileWriter.InitializeFiles();

            switch(status)
            {
                //Success, do nothing.
                case 0:
                    break;

                //Unable to create directory.
                case 1:
                    MessageBox.Show("Unable to create required path to directory, no files will be created.");
                    FileWriter.WriteErrorToLogFile(1);
                    break;

                //Unable to create files.
                case 2:
                    MessageBox.Show("Path was found, but files could not be created.");
                    FileWriter.WriteErrorToLogFile(2);
                    break;
            }

            Sound.InitializeSound();
        }

        /* 
         * Here are key event handlers for keyUp and keyDown events.
         */
        private void Key_Up_Event_Handler(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Down:
                    game_tick_timer.Interval = 1000;
                    break;

                default:
                    break;
            }
        }

        private void Key_Down_Event_Handler(object sender, KeyEventArgs e)
        {
            //First comes key inputs that are global - independent of any view.
            if(e.KeyCode == Keys.M)
            {
                if (Sound.IsMusicPlaying())
                {
                    Sound.Enable(false);
                    return;
                }

                Sound.Enable(true);
            }
            //All key commands available in the main menu.
            if(IWhatToDraw == (int)Draw.MainMenu)
            {
                switch(e.KeyCode)
                {
                    case Keys.Enter:
                        if (iMainMenuCursorPosition == 0) //Play
                        {
                            IWhatToDraw = (int)Draw.Game;
                            InitializeNewGame();
                            game_tick_timer.Start();
                            return;
                        }
                        else if (iMainMenuCursorPosition == 1) //Highscore
                        {
                            return;
                        }
                        else if (iMainMenuCursorPosition == 2) //Options
                        {
                            IWhatToDraw = (int)Draw.Options;
                            iOptionsCursorPosition = 0;
                            return;
                        }
                        else if (iMainMenuCursorPosition == 3) //Quit
                        {
                            Application.Exit();
                        }
                        break;


                    case Keys.Up:
                        if (iMainMenuCursorPosition > 0)
                        {
                            --iMainMenuCursorPosition;
                        }
                        break;
                    case Keys.Down:
                        if(iMainMenuCursorPosition < 3)
                        {
                            ++iMainMenuCursorPosition;
                        }
                        break;
                }
            }
            
            //All key commands available during the game.
            else if (IWhatToDraw == (int)Draw.Game)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if(bGameOver)
                        {
                            InitializeNewGame();
                            game_tick_timer.Start();
                        }
                        break;

                    case Keys.Down:
                        game_tick_timer.Interval = 50;
                        break;

                    case Keys.Left:
                        if (game_tick_timer.Enabled)
                        {
                            MoveBlock((int)Direction.LEFT);
                        }
                        break;

                    case Keys.Right:
                        if (game_tick_timer.Enabled)
                        {
                            MoveBlock((int)Direction.RIGHT);
                        }
                        break;

                    case Keys.R:
                        if (game_tick_timer.Enabled)
                        {
                            activeBlock = Rotate();
                        }
                        break;

                    case Keys.P:
                        if (game_tick_timer.Enabled)
                        {
                            game_tick_timer.Stop();
                            return;
                        }
                        game_tick_timer.Start();
                            break;

                    case Keys.Escape:
                            game_tick_timer.Stop();
                            IWhatToDraw = (int)Draw.MainMenu;
                        break;

                }
            }

            //All key commands available in the options menu.
            else if (IWhatToDraw == (int)Draw.Options && !Graphics.bDisplayKeybindsInOptionsMenu)
            {
                switch(e.KeyCode)
                {
                    case Keys.Escape:
                        IWhatToDraw = (int)Draw.MainMenu;
                        break;

                    case Keys.Up:
                        if(iOptionsCursorPosition > 0)
                        {
                            --iOptionsCursorPosition;
                        }
                        break;
                    case Keys.Down:
                        if(iOptionsCursorPosition < Graphics.optionsText.Length - 1)
                        {
                            ++iOptionsCursorPosition;
                        }
                        break;

                    case Keys.Left:
                        if(iOptionsCursorPosition == 0 && Sound.IsEnabled() == true)
                        {
                            Sound.Enable(false);
                        }
                        else if (iOptionsCursorPosition == 1)
                        {
                            Sound.SetVolumeLevel(false);
                        }
                        else if (iOptionsCursorPosition == 2)
                        {

                        }
                        break;

                    case Keys.Right:
                        if (iOptionsCursorPosition == 0 && Sound.IsEnabled() == false)
                        {
                            Sound.Enable(true);
                        }
                        else if (iOptionsCursorPosition == 1)
                        {
                            Sound.SetVolumeLevel(true);
                        }
                        break;

                    case Keys.Enter:
                        if(iOptionsCursorPosition == 4)
                        {
                            IWhatToDraw = (int)Draw.MainMenu;
                        }
                        else if (iOptionsCursorPosition == 3)
                        {
                            Graphics.bDisplayKeybindsInOptionsMenu = true;
                        }
                        break;
                }
            }

            else if (IWhatToDraw == (int)Draw.Options && Graphics.bDisplayKeybindsInOptionsMenu)
            {
                if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                {
                    Graphics.bDisplayKeybindsInOptionsMenu = false;
                }
            }
        }


        //Not currently used. Variables are initialized in the constructor.
        private void tetris_main_form_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (IWhatToDraw)
            {
                case 0:
                    Graphics.DrawMainMenu(e, iMainMenuCursorPosition);
                    break;
                case 1:
                    Graphics.DrawGame(e, fieldPoint, posX, posY, tetrisMap, activeBlock, nextBlock, iHighscore, bGameOver);
                    break;
                case 2:
                    break;
                case 3:
                    Graphics.DrawOptionsMenu(e, iOptionsCursorPosition);
                    break;
                default:
                    break;
            }
        }

        //Region where all logics are stored for the Tetris game.
        #region Tetris logic

        private static void InitializeNewGame()
        {
            iHighscore = 0;
            bGameOver = false;
            posY = 0;
            posX = 4;

            for (int x = 0; x < 21; ++x)
                for (int y = 0; y < 22; ++y)
                {
                    if (x == 0 || x == 11)
                        tetrisMap[x, y] = BLOCK.BORDER;
                    else if (y == 21 && x < 21)
                        tetrisMap[x, y] = BLOCK.BORDER;
                    else if (y == 6 && x > 11)
                        tetrisMap[x, y] = BLOCK.BORDER;
                    else if (x == 20)
                        tetrisMap[x, y] = BLOCK.BORDER;
                    else
                        tetrisMap[x, y] = BLOCK.EMPTY;
                }

            activeBlock = GenerateNewBlock();
            nextBlock = GenerateNewBlock();
        }

        private static BLOCK[,] GenerateNewBlock()
        {
            int nIndex = rand.Next(0, 7);

            BLOCK[,] newBlock = new BLOCK[4, 4];
            int[] blockType = tetromino[nIndex];

            BLOCK type = (BLOCK)Enum.GetValues(typeof(BLOCK)).GetValue(nIndex + 1);

            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    if (blockType[i + (j * 4)] == 1)
                        newBlock[i, j] = type;
                }

            return newBlock;
        }

        private static BLOCK[,] Rotate()
        {
            BLOCK[,] newBlock = new BLOCK[4, 4];

            //Perform the rotation. After that, we need to ensure its NEW position does not intersect with any border.
            //If so, we need to push it to a suitable side.

            int newColumn, newRow = 0;
            int temp_posX = posX;
            int temp_posY = posY;

            //This does the rotation.
            for (int oldColumn = activeBlock.GetLength(1) - 1; oldColumn >= 0; --oldColumn)
            {
                newColumn = 0;
                for (int oldRow = 0; oldRow < activeBlock.GetLength(0); ++oldRow)
                {
                    newBlock[newRow, newColumn] = activeBlock[oldRow, oldColumn];
                    ++newColumn;
                }

                ++newRow;
            }

            /* ----------------------------------------------------
             * Now we will check the if the new block collides with any other block. If yes, we have to either:
             * 1: Ignore the rotation, or...
             * 2: Push the block.
             * The latter should only be done if we collide with BORDER.
             * 
             * First loops pushes the block out from the border.
             * The second loops checks if the new block intersects with existing blocks. If yes, send back 'activeBlock'.
             * ----------------------------------------------------*/

            //Precaution to avoid checking array place -1, which doesn't exist.
            //Also, only the fully straight tetromino can cause 'posX' to be 0. When we rotate it,
            //we HAVE to set it to at least 0. Otherwise it intersects in the border (since the border starts at 0).
            if (posX < 0) posX = 0;

            //Loops for checking if new block intersect with border.
            for (int column = 0; column < newBlock.GetLength(1); ++column) {
                for (int row = 0; row < newBlock.GetLength(0); ++row) {
                    if ((newBlock[row, column] != BLOCK.EMPTY)
                        && (tetrisMap[row + posX, column + posY] == BLOCK.BORDER)) {
                        if (posX < 5) {
                            ++posX;
                            row = -1;
                        } else if (posX > 5) {
                            --posX;
                            row = -1;
                        }
                    }
                }
            }

            //Loops for checking if newblock collides with any current block AFTER we've moved it from the borders.
            for (int column = 0; column < newBlock.GetLength(1); ++column)
            {
                for (int row = 0; row < newBlock.GetLength(0); ++row)
                {
                    if (newBlock[row, column] > BLOCK.EMPTY && tetrisMap[row + posX, column + posY] > BLOCK.EMPTY)
                    {
                        //Reset the position to before the rotation and send back the current block.
                        posX = temp_posX;
                        posY = temp_posY;
                        return activeBlock;
                    }
                }
            }

            return newBlock;
        }

        private static void MoveBlock(int direction)
        {
            int checkPosX = posX;
            int checkPosY = posY;

            switch (direction)
            {
                case 0: checkPosY += 1; break; //0 = down
                case 1: checkPosX += 1; break; //1 = Right
                case 2: checkPosX -= 1; break;  //2 = Left
                default: break;
            }

            for (int columns = 0; columns < activeBlock.GetLength(0); ++columns)
            {
                for (int rows = 0; rows < activeBlock.GetLength(1); ++rows)
                {
                    if (activeBlock[columns, rows] > BLOCK.EMPTY && tetrisMap[checkPosX + columns, checkPosY + rows] > BLOCK.EMPTY)
                    {
                        return;
                    }
                }
            }


            switch (direction)
            {
                case 0: ++posY; break; //0 = down
                case 1: ++posX; break; //1 = Right
                case 2: --posX; break;  //2 = Left
                default: break;
            }
        }

        private static bool CollidesWithFloorOrAnotherBlock()
        {
            for (int column = 0; column < activeBlock.GetLength(1); ++column)
                for (int row = 0; row < activeBlock.GetLength(0); ++row)
                {
                    if (activeBlock[row, column] > BLOCK.EMPTY && tetrisMap[row + posX, column + posY + 1] > BLOCK.EMPTY)
                        return true;
                }

            return false;
        }

        private static void CheckBlocksInRow()
        {
            int nTotalFullRows = 0;

            int checkRow = tetrisMap.GetLength(1) - 2, checkColumn = 1;

            while (checkRow > 0)
            {
                for (; checkColumn < 11; ++checkColumn)
                {
                    if (tetrisMap[checkColumn, checkRow] == BLOCK.EMPTY)
                        break;

                    if (checkColumn == 10)
                    {
                        for (int clearRow = checkRow; clearRow > 1; --clearRow)
                        {
                            for (int clearColumn = 1; clearColumn < 11; ++clearColumn)
                            {
                                tetrisMap[clearColumn, clearRow] = tetrisMap[clearColumn, clearRow - 1];
                            }
                        }
                        ++checkRow;
                        ++nTotalFullRows;
                    }
                }

                checkColumn = 1;
                --checkRow;
            }

            iHighscore += nTotalFullRows;
        }

        public static void PlaceBlock()
        {
            for (int column = 0; column < activeBlock.GetLength(1); ++column)
                for (int row = 0; row < activeBlock.GetLength(0); ++row)
                {
                    if (activeBlock[row, column] > BLOCK.EMPTY)
                        tetrisMap[row + posX, column + posY] = activeBlock[row, column];
                }
        }

        #endregion

        //This timer should check if we collide with a block at the start of every tick.
        //If true, place the block and generate a new one.
        //
        private void game_tick_timer_Tick(object sender, EventArgs e)
        {
            if(CollidesWithFloorOrAnotherBlock())
            {
                PlaceBlock();
                CheckBlocksInRow();
                activeBlock = nextBlock;
                nextBlock = GenerateNewBlock();
                posX = 4;
                posY = 0;

                if(CollidesWithFloorOrAnotherBlock())
                {
                    bGameOver = true;
                    game_tick_timer.Stop();
                }

                return;
            }
            MoveBlock((int)Direction.DOWN);
        }

        private void draw_refresh_timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
