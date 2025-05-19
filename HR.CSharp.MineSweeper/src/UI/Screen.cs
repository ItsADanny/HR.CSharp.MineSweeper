public static class Screen
{
    private static string _gameLogo = "▗▖  ▗▖▗▄▄▄▖▗▖  ▗▖▗▄▄▄▖     ▗▄▄▖▗▖ ▗▖▗▄▄▄▖▗▄▄▄▖▗▄▄▖ ▗▄▄▄▖▗▄▄▖ \n" +
                                     "▐▛▚▞▜▌  █  ▐▛▚▖▐▌▐▌       ▐▌   ▐▌ ▐▌▐▌   ▐▌   ▐▌ ▐▌▐▌   ▐▌ ▐▌\n" +
                                     "▐▌  ▐▌  █  ▐▌ ▝▜▌▐▛▀▀▘     ▝▀▚▖▐▌ ▐▌▐▛▀▀▘▐▛▀▀▘▐▛▀▘ ▐▛▀▀▘▐▛▀▚▖\n" +
                                     "▐▌  ▐▌▗▄█▄▖▐▌  ▐▌▐▙▄▄▖    ▗▄▄▞▘▐▙█▟▌▐▙▄▄▖▐▙▄▄▖▐▌   ▐▙▄▄▖▐▌ ▐▌";
    private static string _bar = TerminalUIKit.TextGeneration(61, "=");
    private static string _standardHeader = $"{_bar}\n{_gameLogo}\n{_bar}\n";
    private static string _controls = "↑     : Move up\n↓     : Move down\n←     : Move Left\n→     : Move right\nENTER : Interact";

    public static int StartMenu()
    {
        string Footer = $"\n{_bar}\n{TerminalUIKit.TextGeneration(27)}Game By\n{TerminalUIKit.TextGeneration(24)}Danny de Snoo\n{_bar}";
        List<string> options = [
            "Start",
            "High scores",
            "Credits",
            "Exit"
        ];
        return TerminalUIKit.OptionSelector(_standardHeader, Footer, options);
    }

    public static int SelectDifficulty()
    {
        List<string> options = [
            "Beginner",
            "Intermediate",
            "Expert",
            "Custom"
        ];
        return TerminalUIKit.OptionSelector(_standardHeader + "Please select an Difficulty:\n", null, options);
    }

    public static int[] GetCustomGame()
    {
        int BoardSizeX = TerminalUIKit.IntSelector(_standardHeader + "Please select the game board width:\n", 9, 30);
        int BoardSizeY = TerminalUIKit.IntSelector(_standardHeader + $"Please select the game board height:\n{BoardSizeX}\nPlease select the game board width:\n", 9, 30);
        int BombsAmount = TerminalUIKit.IntSelector(_standardHeader + $"Please select the game board height:\n{BoardSizeX}\nPlease select the game board width:\n{BoardSizeY}\nPlease select the amount of bombs you want:\n", 10, 36);
        return new int[] {
            BoardSizeX,
            BoardSizeY,
            BombsAmount
        };
    }

    public static void ShowHighScores()
    {
        Console.Clear();
        Console.WriteLine(_standardHeader);
    }

    public static void ShowCredits()
    {

    }

    public static void ShowExit()
    {
        Console.Clear();
        Console.WriteLine(_standardHeader);
        Console.WriteLine("Thank you for playing!");
    }

    public static void PrintDisplay(int screenSizeX, int screenSizeY, Bomb[] bombs, Spot[] clearSpots, int cursorPosX, int cursorPosY)
    {
        for (int y = 0; y < screenSizeY; y++)
        {
            string row = "";
            for (int x = 0; x < screenSizeX; x++)
            {
                if (cursorPosX == x && cursorPosY == y)
                {
                    row += "\x1b[44m";
                }

                bool AlreadySomethingInPlace = false;

                foreach (Bomb bomb in bombs)
                {
                    if (bomb.X == x && bomb.Y == y && bomb.Show)
                    {
                        AlreadySomethingInPlace = true;
                        row += "💣";
                    }
                }

                if (!AlreadySomethingInPlace)
                {
                    foreach (Spot spot in clearSpots)
                    {
                        if (spot.X == x && spot.Y == y && spot.Show)
                        {
                            AlreadySomethingInPlace = true;
                            row += " ";
                        }
                    }
                }

                if (!AlreadySomethingInPlace)
                {
                    row += "?";
                }

                if (cursorPosX == x && cursorPosY == y)
                {
                    row += "\x1b[49m";
                }
            }

            Console.WriteLine(row);
        }
    }

    public static void PrintBar(int barLength, bool checkControlLength=false) {
        if (checkControlLength) {
            int MaxControlLength = _mGetMaxLengthControlsStr();
            if (MaxControlLength > barLength)
            {
                Console.WriteLine(TerminalUIKit.TextGeneration(MaxControlLength, "="));
            }
            else
            {
                Console.WriteLine(TerminalUIKit.TextGeneration(barLength, "="));
            }
        } else { 
            Console.WriteLine(TerminalUIKit.TextGeneration(barLength, "="));
        }
    }

    private static int _mGetMaxLengthControlsStr()
    {
        string[] strings = _controls.Split("\n");
        int returnValue = 0;
        foreach (string str in strings)
        {
            if (str.Length > returnValue)
            {
                returnValue = str.Length;
            }
        }

        return returnValue;
    }

    public static void PrintControls() => Console.WriteLine(_controls);

    public static void Clear() => Console.Clear();

    public static void PrintGameScreen(int[] gameBoardInfo, Bomb[] bombs, Spot[] clearSpots, int cursorPosX, int cursorPosY, int points, int stepsPlayed)
    { 
        Clear();
        PrintBar(gameBoardInfo[0], true);
        Console.WriteLine($"Points    : {points}");
        Console.WriteLine($"Moves made: {stepsPlayed}");
        PrintBar(gameBoardInfo[0], true);
        PrintDisplay(gameBoardInfo[0], gameBoardInfo[1], bombs, clearSpots, cursorPosX, cursorPosY);
        PrintBar(gameBoardInfo[0], true);
        PrintControls();
        PrintBar(gameBoardInfo[0], true);
    }
}