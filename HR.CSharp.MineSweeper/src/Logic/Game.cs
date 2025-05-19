public static class Game
{

    private static Dictionary<int, int[]> _difficultiesData = new Dictionary<int, int[]> {
        {0, new int[] {9, 9, 10}},
        {1, new int[] {16, 16, 40}},
        {2, new int[] {30, 16, 99}}
    };

    private enum _difficulties
    {
        Beginner,
        Intermediate,
        Expert,
        Custom
    }

    public static void Start()
    {
        bool Exit = false;
        while (!Exit)
            switch (Screen.StartMenu())
            {
                case 0:
                    int selectedDifficulty = Screen.SelectDifficulty();
                    if (selectedDifficulty == 3)
                    {
                        int[] customGameInfo = Screen.GetCustomGame();
                        _mEnterGameLoop(selectedDifficulty, customGameInfo);
                    }
                    else
                    {
                        _mEnterGameLoop(selectedDifficulty);
                    }
                    break;
                case 1:
                    Screen.ShowHighScores();
                    break;
                case 2:
                    Screen.ShowCredits();
                    break;
                case 3:
                    Exit = true;
                    break;
            }
        Screen.ShowExit();
    }

    private static void _mEnterGameLoop(int difficulty, int[]? customGameInfo = null)
    {
        int[]? GameBoardInfo = null;

        if (customGameInfo is null)
        {
            GameBoardInfo = _difficultiesData[difficulty];
        }
        else
        {
            GameBoardInfo = customGameInfo;
        }

        int StepsPlayed = 0;
        Bomb[] GameBombs = _mGenerateBombs(GameBoardInfo);
        Spot[] ClearSpots = _mGenerateClearSpots(GameBoardInfo, GameBombs);
        bool StillAlive = true;
        bool GameOver = false;
        int CursorPosX = 0;
        int CursorPosY = 0;
        int Points = 0;

        while (!GameOver)
        {
            Screen.PrintGameScreen(GameBoardInfo, GameBombs, ClearSpots, CursorPosX, CursorPosY, Points, StepsPlayed);

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    if (CursorPosY > 0)
                    {
                        CursorPosY--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (CursorPosY < GameBoardInfo[1])
                    {
                        CursorPosY++;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (CursorPosX < GameBoardInfo[0])
                    {
                        CursorPosX++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (CursorPosX > 0)
                    {
                        CursorPosX--;
                    }
                    break;
                case ConsoleKey.Enter:
                    //Implement the feature that it checks if there is a bomb in that spot and if not then open all the clear spots around it
                    int bombResult = _mBombLoc(GameBombs, CursorPosX, CursorPosY);
                    if (bombResult >= 0)
                    {
                        GameBombs[bombResult].Found();
                        StillAlive = false;
                        GameOver = true;
                    }
                    else
                    {

                    }
                    StepsPlayed++;
                    break;
            }
        }

        //TEMP, THIS WILL GET ITS OWN SPECIAL PARTS, THIS IS JUST TO SEE WHAT HAPPENS WHEN WE HAVE HIT A MINE
        Screen.PrintGameScreen(GameBoardInfo, GameBombs, ClearSpots, CursorPosX, CursorPosY, Points, StepsPlayed);
        Thread.Sleep(1000000);

        if (StillAlive)
        {

        }
        else
        {

        }
    }

    private static Bomb[] _mGenerateBombs(int[] gameBoardInfo)
    {
        List<Bomb> bombs = [];

        while (bombs.Count != gameBoardInfo[2])
        {
            int[] tempBomb = _mGenBomb(gameBoardInfo);
            bool alreadyBombInLoc = false;
            foreach (Bomb bomb in bombs)
            {
                if (!alreadyBombInLoc)
                {
                    if (tempBomb[0] == bomb.X && tempBomb[1] == bomb.Y)
                    {
                        alreadyBombInLoc = true;
                    }
                }
            }

            if (!alreadyBombInLoc)
            {
                bombs.Add(new Bomb(tempBomb[0], tempBomb[1]));
            }
        }
        ;

        return bombs.ToArray();
    }

    private static int[] _mGenBomb(int[] gameBoardInfo)
    {
        Random random = new Random();
        int LocX = random.Next(0, gameBoardInfo[0]);
        int LocY = random.Next(0, gameBoardInfo[1]);
        return new int[] {
            LocX,
            LocY
        };
    }

    private static Spot[] _mGenerateClearSpots(int[] gameBoardInfo, Bomb[] bombs)
    {
        List<Spot> returnValue = [];
        for (int y = 0; y < gameBoardInfo[1]; y++)
        {
            for (int x = 0; x < gameBoardInfo[1]; x++)
            {
                bool isBombLocation = false;
                foreach (Bomb bomb in bombs)
                {
                    if (bomb.Y == y && bomb.X == x)
                    {
                        isBombLocation = true;
                    }
                }

                if (!isBombLocation)
                {
                    returnValue.Add(new Spot(x, y));
                }
            }
        }

        return returnValue.ToArray();
    }

    private static int _mBombLoc(Bomb[] bombs, int cursorPosX, int cursorPosY)
    {
        int returnValue = -1;
        int pos = 0;
        foreach (Bomb bomb in bombs)
        {
            if (bomb.X == cursorPosX && bomb.Y == cursorPosY)
            {
                returnValue = pos;
            }
            pos++;
        }
        return returnValue;
    }
}