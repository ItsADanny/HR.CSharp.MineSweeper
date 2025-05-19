using System.Data;
using System.Globalization;

public static class TerminalUIKit {
    public static int IntSelector(string? header=null, int minValue=1, int maxValue=5, int startInAmount=999999999, string? icon=null, int step=1) {
        bool done = false;
        int returnValue = 0;
        if (startInAmount == 999999999) {
            startInAmount = minValue;
        }
        int selectedAmount = startInAmount;
        int pos = 0;
        
        do {
            Console.Clear();

            if (header is not null) {
                Console.WriteLine(header);
            }
            
            string row = "";

            if (icon is not null) {
                string icons = "";
                for (int x = 0; x != selectedAmount; x++) {
                    icons += icon;
                }
                Console.WriteLine(icons);
                
                row += "   ";
            }

            for (int x = 0; x != 3; x++) {
                if (x == pos) {
                    row += "\x1b[44m";
                }

                if (x == 0) {
                    row += "<\x1b[49m";
                } else if (x == 2) {
                    row += ">\x1b[49m";
                } else {
                    row += $" {selectedAmount} ";
                }
            }
            Console.WriteLine(row);

            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.LeftArrow:
                    if (pos == 2) {
                        pos = 0;
                    } else {
                        pos = 2;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (pos == 2) {
                        pos = 0;
                    } else {
                        pos = 2;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    if (pos == 0) {
                        if (selectedAmount > minValue) {
                            selectedAmount -= step;
                        }
                    } else {
                        if (selectedAmount < maxValue) {
                            selectedAmount += step;
                        }
                    }
                    break;
                case ConsoleKey.Enter:
                    done = true;
                    returnValue = selectedAmount;
                    break;
            }
        } while (!done);

        return returnValue;
    }

    public static TimeOnly TimeSelector(string Header, string startTime="00:00", string minValue="00:01", string maxValue="24:00") {
        TimeOnly time = TimeOnly.FromDateTime(DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture));
        bool Done = false;
        string? errorMessage = null;
        int posY = 0;
        int posX = 0;

        do {
            Console.Clear();
            Console.WriteLine(Header);

            if (errorMessage is not null) {
                Console.WriteLine(errorMessage);
                errorMessage = null;
            }

            for (int y = 0; y != 3; y++) {
                string row = "";
                if (y == 0) {
                    for (int x = 0; x != 5; x++) {
                        if (x == 2) {
                            row += " ";
                        } else {
                            if (posX == x & posY == y) {
                                row += "\x1b[44m";
                            }
                            row += "↑\x1b[49m";
                        }
                    }
                } else if (y == 1) {
                    row = time.ToString("HH:mm");
                } else {
                    for (int x = 0; x != 5; x++) {
                        if (x == 2) {
                            row += " ";
                        } else {
                            if (posX == x & posY == y) {
                                row += "\x1b[44m";
                            }
                            row += "↓\x1b[49m";
                        }
                    }
                }
                Console.WriteLine(row);
            }

            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.UpArrow:
                    if (posY == 2) {
                        posY = 0;
                    } else {
                        posY = 2;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (posY == 0) {
                        posY = 2;
                    } else {
                        posY = 0;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    posX--;
                    if (posX == 2) {
                        posX--;
                    }
                    if (posX < 0) {
                        posX = 5;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    posX++;
                    if (posX == 2) {
                        posX++;
                    }
                    if (posX > 5) {
                        posX = 0;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    Dictionary<int, string> dict_posHourMinute = new Dictionary<int, string> {
                            {0, "H"}, {1, "H"},
                            {2, "S"},
                            {3, "M"}, {4, "M"}
                    };

                    if (dict_posHourMinute[posX] == "H") {
                            if (posY == 0) {
                                if (posX == 0) {
                                    time = time.AddHours(10);
                                } else {
                                    time = time.AddHours(1);
                                }
                            } else {
                                if (posX == 0) {
                                    time = time.AddHours(-10);
                                } else {
                                    time = time.AddHours(-1);
                                }
                            }
                    } else if (dict_posHourMinute[posX] == "M") {
                        if (posY == 0) {
                                if (posX == 3) {
                                    time = time.AddMinutes(10);
                                } else {
                                    time = time.AddMinutes(1);
                                }
                            } else {
                                if (posX == 3) {
                                    time = time.AddMinutes(-10);
                                } else {
                                    time = time.AddMinutes(-1);
                                }
                            }
                    }
                    break;
                case ConsoleKey.C:
                    time = TimeOnly.FromDateTime(DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture));
                    break;
                case ConsoleKey.Enter:
                    Done = true;
                    break;
            }

        } while (!Done);

        return time;
    }

    public static DateOnly DateSelector(string? Header=null, string? Footer=null, bool CanGoIntoThePast=false) {
        string str_date = DateTime.Now.ToString("dd/MM/yyyy");
        string? selectedDate = null;
        string? errorMessage = null;
        int posY = 0;
        int posX = 0;

        do {
            Console.Clear();
            if (Header is not null) {
                Console.WriteLine(Header);
            }

            if (errorMessage is not null) {
                Console.WriteLine(errorMessage);
                errorMessage = null;
            }

            for (int y = 0; y != 3; y++) {
                string row = "";
                if (y == 0) {
                    for (int x = 0; x != str_date.Length; x++) {
                        if (x == 2 | x == 5) {
                            row += " ";
                        } else {
                            if (posX == x & posY == y) {
                                row += "\x1b[44m";
                            }
                            row += "↑\x1b[49m";
                        }
                    }
                } else if (y == 1) {
                    row = str_date;
                } else {
                    for (int x = 0; x != str_date.Length; x++) {
                        if (x == 2 | x == 5) {
                            row += " ";
                        } else {
                            if (posX == x & posY == y) {
                                row += "\x1b[44m";
                            }
                            row += "↓\x1b[49m";
                        }
                    }
                }
                Console.WriteLine(row);
            }

            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.UpArrow:
                    if (posY == 2) {
                        posY = 0;
                    } else {
                        posY = 2;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (posY == 0) {
                        posY = 2;
                    } else {
                        posY = 0;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    posX--;
                    if (posX == 2 | posX == 5) {
                        posX--;
                    }
                    if (posX < 0) {
                        posX = 9;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    posX++;
                    if (posX == 2 | posX == 5) {
                        posX++;
                    }
                    if (posX > 9) {
                        posX = 0;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    try {
                        Dictionary<int, string> dict_posDayMonthYear = new Dictionary<int, string> {
                            {0, "D"}, {1, "D"},
                            {2, "S"},
                            {3, "M"}, {4, "M"},
                            {5, "S"},
                            {6, "Y"}, {7, "Y"}, {8, "Y"}, {9, "Y"}
                        };

                        DateTime date = DateTime.ParseExact(str_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        switch (dict_posDayMonthYear[posX]) {
                            case "D":
                                if (posY == 0) {
                                    if (posX == 0) {
                                        date = date.AddDays(10);
                                    } else {
                                        date = date.AddDays(1);
                                    }
                                } else {
                                    if (posX == 0) {
                                        date = date.AddDays(-10);
                                    } else {
                                        date = date.AddDays(-1);
                                    }
                                }
                                break;
                            case "M":
                                if (posY == 0) {
                                    if (posX == 3) {
                                        date = date.AddMonths(10);
                                    } else {
                                        date = date.AddMonths(1);
                                    }
                                } else {
                                    if (posX == 3) {
                                        date = date.AddMonths(-10);
                                    } else {
                                        date = date.AddMonths(-1);
                                    }
                                }
                                break;
                            case "Y":
                                if (posY == 0) {
                                    if (posX == 6) {
                                        date = date.AddYears(1000);
                                    } else if (posX == 7) {
                                        date = date.AddYears(100);
                                    } else if (posX == 8) {
                                        date = date.AddYears(10);
                                    } else {
                                        date = date.AddYears(1);
                                    }
                                } else {
                                    if (posX == 6) {
                                        date = date.AddYears(-1000);
                                    } else if (posX == 7) {
                                        date = date.AddYears(-100);
                                    } else if (posX == 8) {
                                        date = date.AddYears(-10);
                                    } else {
                                        date = date.AddYears(-1);
                                    }
                                }
                                break;
                        }
                        if (!CanGoIntoThePast) {
                            if (date < DateTime.Now) {
                                errorMessage = "You can't select a date in the past, please select a date in the future";
                                date = DateTime.Now;
                            }
                        }
                        
                        str_date = date.ToString("dd/MM/yyyy");
                    }
                    catch (System.Exception)
                    {
                        errorMessage = "You tried to select an invalid date, please select a date";
                    }
                    break;
                case ConsoleKey.C:
                    str_date = DateTime.Now.ToString("dd/MM/yyyy");
                    break;
                case ConsoleKey.Enter:
                    selectedDate = str_date;
                    break;
            }
        } while (selectedDate is null);

        return DateOnly.FromDateTime(DateTime.ParseExact(selectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
    }

    public static Dictionary<string, bool> CheckBoxSelector(string? Header=null, string? Footer=null, List<string>? options_string=null) {
        if (options_string is null) throw new Exception("No options inputted, to use this function please add options to select");

        List<bool> options_bool = new ();
        for (int i = 0; i != options_string.Count(); i++) {
            options_bool.Add(false);
        }

        bool done = false;
        int pos = 0;

        do {
            Console.Clear();
            
            if (Header is not null) {
                Console.WriteLine(Header);
            }

            for (int y = 0; y != options_string.Count(); y++) {
                string row = "";
                if (pos == y) {
                    row += "\x1b[44m>";
                } else {
                    row += " ";
                }

                if (options_bool[y]) {
                    row += " [X] ";
                } else {
                    row += " [ ] ";
                }

                row += $" {options_string[y]}\x1b[49m";
                Console.WriteLine(row);
            }

            if (Footer is not null) {
                Console.WriteLine(Footer);
            }

            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.UpArrow:
                    if (pos <= 0) {
                        pos = options_string.Count() - 1;
                    } else {
                        pos -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (pos >= (options_string.Count() - 1)) {
                        pos = 0;
                    } else {
                        pos += 1;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    if (options_bool[pos]) {
                        options_bool[pos] = false;
                    } else {
                        options_bool[pos] = true;
                    }
                    break;
                case ConsoleKey.Enter:
                    done = true;
                    break;
            }
        } while (!done);

        int curr_pos = 0;
        Dictionary<string, bool> returnDict = new ();
        foreach (string option in options_string) {
            returnDict.Add(option, options_bool[curr_pos]);
            curr_pos++;
        }
        return returnDict;
    }

    public static Dictionary<string, bool> CheckBoxSelector(string? Header=null, string? Footer=null, Dictionary<string, bool>? options=null) {
        if (options is null) throw new Exception("No options inputted, to use this function please add options to select");

        List<string> options_string = new ();
        List<bool> options_bool = new ();
        foreach (var KeyPairValueItem in options) {
            options_string.Add(KeyPairValueItem.Key);
            options_bool.Add(KeyPairValueItem.Value);
        }

        bool done = false;
        int pos = 0;

        do {
            Console.Clear();

            if (Header is not null) {
                Console.WriteLine(Header);
            }
            
            for (int y = 0; y != options_string.Count(); y++) {
                string row = "";
                if (pos == y) {
                    row += "\x1b[44m>";
                } else {
                    row += " ";
                }

                if (options_bool[y]) {
                    row += " [X] ";
                } else {
                    row += " [ ] ";
                }

                row += $" {options_string[y]}\x1b[49m";
                Console.WriteLine(row);
            }

            if (Footer is not null) {
                Console.WriteLine(Footer);
            }
                
            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.UpArrow:
                    if (pos <= 0) {
                        pos = options_string.Count() - 1;
                    } else {
                        pos -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (pos >= (options_string.Count() - 1)) {
                        pos = 0;
                    } else {
                        pos += 1;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    if (options_bool[pos]) {
                        options_bool[pos] = false;
                    } else {
                        options_bool[pos] = true;
                    }
                    break;
                case ConsoleKey.Enter:
                    done = true;
                    break;
            }
        } while (!done);

        int curr_pos = 0;
        Dictionary<string, bool> returnDict = new ();
        foreach (string option in options_string) {
            returnDict.Add(option, options_bool[curr_pos]);
            curr_pos++;
        }
        return returnDict;
    }

    public static int OptionSelector(string? Header=null, string? Footer=null, List<string>? options=null) {
        if (options is null) throw new Exception("No options inputted, to use this function please add options to select");

        int selectedOption = 999999999;
        int pos = 0;

        do {
            Console.Clear();
            if (Header is not null) {
                Console.WriteLine(Header);
            }
            
            for (int y = 0; y != options.Count(); y++) {
                string row = "";
                if (pos == y) {
                    row += "\x1b[44m>";
                } else {
                    row += " ";
                }

                row += $" {options[y]}\x1b[49m";
                Console.WriteLine(row);
            }

            if (Footer is not null) {
                Console.WriteLine(Footer);
            }

            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.UpArrow:
                    if (pos <= 0) {
                        pos = options.Count() - 1;
                    } else {
                        pos -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (pos >= (options.Count() - 1)) {
                        pos = 0;
                    } else {
                        pos += 1;
                    }
                    break;
                case ConsoleKey.Enter:
                    selectedOption = pos;
                    break;
            }
        } while (selectedOption == 999999999);
        return selectedOption;
    }

    public static string TextBox(string? Header=null, string? Footer=null, int? MinRequiredLength=null, int? MaxRequiredLength=null, List<string>? CantContain=null) {
        bool valid = false;
        string currentString = "";
        string errorMessage = "";
        do {
            Console.Clear();

            if (Header is not null) {
                Console.WriteLine(Header);
            }
            
            Console.WriteLine(currentString);

            if (errorMessage != "") {
                Console.WriteLine(errorMessage);
                errorMessage = "";
            }
            if (Footer is not null) {
                Console.WriteLine(Footer);
            }

            var input = Console.ReadKey();
            switch (input.Key) {
                case ConsoleKey.A:
                    if (UsedShift(input)) {
                        currentString += "A";
                    } else {
                        currentString += "a";
                    }
                break;
                case ConsoleKey.B:
                    if (UsedShift(input)) {
                        currentString += "B";
                    } else {
                        currentString += "b";
                    }
                break;
                case ConsoleKey.C:
                    if (UsedShift(input)) {
                        currentString += "C";
                    } else {
                        currentString += "c";
                    }
                break;
                case ConsoleKey.D:
                    if (UsedShift(input)) {
                        currentString += "D";
                    } else {
                        currentString += "d";
                    }
                break;
                case ConsoleKey.E:
                    if (UsedShift(input)) {
                        currentString += "E";
                    } else {
                        currentString += "e";
                    }
                break;
                case ConsoleKey.F:
                    if (UsedShift(input)) {
                        currentString += "F";
                    } else {
                        currentString += "f";
                    }
                break;
                case ConsoleKey.G:
                    if (UsedShift(input)) {
                        currentString += "G";
                    } else {
                        currentString += "g";
                    }
                break;
                case ConsoleKey.H:
                    if (UsedShift(input)) {
                        currentString += "H";
                    } else {
                        currentString += "h";
                    }
                break;
                case ConsoleKey.I:
                    if (UsedShift(input)) {
                        currentString += "I";
                    } else {
                        currentString += "i";
                    }
                break;
                case ConsoleKey.J:
                    if (UsedShift(input)) {
                        currentString += "J";
                    } else {
                        currentString += "j";
                    }
                break;
                case ConsoleKey.K:
                    if (UsedShift(input)) {
                        currentString += "K";
                    } else {
                        currentString += "k";
                    }
                break;
                case ConsoleKey.L:
                    if (UsedShift(input)) {
                        currentString += "L";
                    } else {
                        currentString += "l";
                    }
                break;
                case ConsoleKey.M:
                    if (UsedShift(input)) {
                        currentString += "M";
                    } else {
                        currentString += "m";
                    }
                break;
                case ConsoleKey.N:
                    if (UsedShift(input)) {
                        currentString += "N";
                    } else {
                        currentString += "n";
                    }
                break;
                case ConsoleKey.O:
                    if (UsedShift(input)) {
                        currentString += "O";
                    } else {
                        currentString += "o";
                    }
                break;
                case ConsoleKey.P:
                    if (UsedShift(input)) {
                        currentString += "P";
                    } else {
                        currentString += "p";
                    }
                break;
                case ConsoleKey.Q:
                    if (UsedShift(input)) {
                        currentString += "Q";
                    } else {
                        currentString += "q";
                    }
                break;
                case ConsoleKey.R:
                    if (UsedShift(input)) {
                        currentString += "R";
                    } else {
                        currentString += "r";
                    }
                break;
                case ConsoleKey.S:
                    if (UsedShift(input)) {
                        currentString += "S";
                    } else {
                        currentString += "s";
                    }
                break;
                case ConsoleKey.T:
                    if (UsedShift(input)) {
                        currentString += "T";
                    } else {
                        currentString += "t";
                    }
                break;
                case ConsoleKey.U:
                    if (UsedShift(input)) {
                        currentString += "U";
                    } else {
                        currentString += "u";
                    }
                break;
                case ConsoleKey.V:
                    if (UsedShift(input)) {
                        currentString += "V";
                    } else {
                        currentString += "v";
                    }
                break;
                case ConsoleKey.W:
                    if (UsedShift(input)) {
                        currentString += "W";
                    } else {
                        currentString += "w";
                    }
                break;
                case ConsoleKey.X:
                    if (UsedShift(input)) {
                        currentString += "X";
                    } else {
                        currentString += "x";
                    }
                break;
                case ConsoleKey.Y:
                    if (UsedShift(input)) {
                        currentString += "Y";
                    } else {
                        currentString += "y";
                    }
                break;
                case ConsoleKey.Z:
                    if (UsedShift(input)) {
                        currentString += "Z";
                    } else {
                        currentString += "z";
                    }
                    break;
                case ConsoleKey.OemPeriod:
                    currentString += ".";
                    break;
                case ConsoleKey.NumPad0: case ConsoleKey.D0:
                    currentString += "0";
                    break;
                case ConsoleKey.Oem1: case ConsoleKey.NumPad1: case ConsoleKey.D1:
                    currentString += "1";
                    break;
                case ConsoleKey.Oem2: case ConsoleKey.NumPad2: case ConsoleKey.D2:
                    currentString += "2";
                    break;
                case ConsoleKey.Oem3: case ConsoleKey.NumPad3: case ConsoleKey.D3:
                    currentString += "3";
                    break;
                case ConsoleKey.Oem4: case ConsoleKey.NumPad4: case ConsoleKey.D4:
                    currentString += "4";
                    break;
                case ConsoleKey.Oem5: case ConsoleKey.NumPad5: case ConsoleKey.D5:
                    currentString += "5";
                    break;
                case ConsoleKey.Oem6: case ConsoleKey.NumPad6: case ConsoleKey.D6:
                    currentString += "5";
                    break;
                case ConsoleKey.Oem7: case ConsoleKey.NumPad7: case ConsoleKey.D7:
                    currentString += "5";
                    break;
                case ConsoleKey.Oem8: case ConsoleKey.NumPad8: case ConsoleKey.D8:
                    currentString += "8";
                    break;
                case ConsoleKey.NumPad9: case ConsoleKey.D9:
                    currentString += "9";
                    break;
                case ConsoleKey.Add: case ConsoleKey.OemPlus:
                    currentString += "+";
                    break;
                case ConsoleKey.Subtract: case ConsoleKey.OemMinus:
                    currentString += "-";
                    break;
                case ConsoleKey.Decimal: case ConsoleKey.OemComma:
                    currentString += ",";
                    break;
                case ConsoleKey.Divide:
                    currentString += "/";
                    break;
                case ConsoleKey.Multiply:
                    currentString += "*";
                    break;
                case ConsoleKey.Spacebar:
                    currentString += " ";
                    break;
                case ConsoleKey.Backspace: case ConsoleKey.Delete:
                    if (currentString.Length > 0) {
                        currentString = currentString.Remove(currentString.Length - 1);
                    }
                    break;
                case ConsoleKey.Clear: case ConsoleKey.OemClear: case ConsoleKey.Escape:
                    currentString = "";
                    break;
                case ConsoleKey.Enter:
                    errorMessage = TextBoxCheck(currentString, MinRequiredLength, MaxRequiredLength, CantContain);

                    if (errorMessage == "") {
                        valid = true;
                    } else {
                        errorMessage += "\nPlease enter a valid input.";
                    }
                    break;
                default:
                    errorMessage += "You pressed a key which can't be used, please use supported keys for your input.";
                    break;
            }
        } while (!valid);
        return currentString;
    }

    public static void DictionaryViewer<T>(string? Header=null, string? Footer=null, Dictionary<string, T>? DictionaryToView=null) {
        if (DictionaryToView is null) return;

        int maxSizeForKey = 0;
        int maxSizeForValue = 0;
        foreach (KeyValuePair<string, T> keyValueItem in DictionaryToView) {
            int sizeKey = keyValueItem.Key.Count();
            int sizeValue = 0;
            if (keyValueItem.Value is not null) {
                sizeValue = Convert.ToString(keyValueItem.Value).Count();
            } else {
                sizeValue = 0;
            }
            
            if (sizeKey > maxSizeForKey) {
                maxSizeForKey = sizeKey;
            }

            if (sizeValue > maxSizeForValue) {
                maxSizeForValue = sizeValue;
            }
        }
        maxSizeForKey += 2;
        maxSizeForValue += 2;

        string topAndBottomBar = TextGeneration(maxSizeForKey + maxSizeForValue + 3, "=");
        string dividerBar = $"|{TextGeneration(maxSizeForKey + maxSizeForValue + 1, "-")}|";
        string outputToPrint = $"{topAndBottomBar}\n";

        int pos = 0;
        foreach (KeyValuePair<string, T> keyValueItem in DictionaryToView) {
            KeyValuePair<string, string> spacesFrontAndBackKey = new KeyValuePair<string, string>();
            KeyValuePair<string, string> spacesFrontAndBackValue = new KeyValuePair<string, string>();
            if (IsEven(maxSizeForKey)) {
                int spacesrequired = maxSizeForKey - Convert.ToString(keyValueItem.Key).Count();
                spacesFrontAndBackKey = new KeyValuePair<string, string>(TextGeneration(spacesrequired / 2), TextGeneration(spacesrequired / 2));
            } else {
                int spacesrequired = maxSizeForKey - Convert.ToString(keyValueItem.Key).Count();
                if (IsEven(spacesrequired)) {
                    spacesFrontAndBackKey = new KeyValuePair<string, string>(TextGeneration(spacesrequired / 2), TextGeneration(spacesrequired / 2));
                } else {
                    int spacesFront = (int) (Convert.ToDouble(spacesrequired) / 2.0);
                    int spacesBack = Convert.ToInt32(Convert.ToDouble(spacesrequired) / 2.0);
                    spacesFrontAndBackKey = new KeyValuePair<string, string>(TextGeneration(spacesFront), TextGeneration(spacesBack));
                }
            }
            string value = "";
            if (keyValueItem.Value is not null) {
                value = keyValueItem.Value.ToString();
            }

            if (IsEven(maxSizeForValue)) {
                int spacesrequired = maxSizeForValue - value.Count();
                spacesFrontAndBackValue = new KeyValuePair<string, string>(TextGeneration(spacesrequired / 2), TextGeneration(spacesrequired / 2));
            } else {
                int spacesrequired = maxSizeForValue - value.Count();
                if (IsEven(spacesrequired)) {
                    spacesFrontAndBackValue = new KeyValuePair<string, string>(TextGeneration(spacesrequired / 2), TextGeneration(spacesrequired / 2));
                } else {
                    int spacesFront = (int) (Convert.ToDouble(spacesrequired) / 2.0);
                    int spacesBack = Convert.ToInt32(Convert.ToDouble(spacesrequired) / 2.0);
                    spacesFrontAndBackValue = new KeyValuePair<string, string>(TextGeneration(spacesFront), TextGeneration(spacesBack));
                }
            }

            outputToPrint += $"|{spacesFrontAndBackKey.Key}{keyValueItem.Key}{spacesFrontAndBackKey.Value}|{spacesFrontAndBackValue.Key}{keyValueItem.Value}{spacesFrontAndBackValue.Value}|\n";
            if (pos != DictionaryToView.Count) {
                outputToPrint += $"{dividerBar}\n";
            }   
            pos++;
        }

        Console.Clear();
        outputToPrint += topAndBottomBar;
        if (Header is not null) {
            Console.WriteLine(Header);
        }
        Console.WriteLine(outputToPrint);
        if (Footer is not null) {
            Console.WriteLine(Footer);
        }
        Console.ReadLine();
    }

    public static void DictionaryViewer<T>(string Header=null, string Footer=null, Dictionary<int, T> DictionaryToView=null) {
        if (DictionaryToView is null) return;

        Dictionary<string, T> ConvertedDictionaryToView = new Dictionary<string, T>();
        foreach (KeyValuePair<int, T> Item in DictionaryToView) {
            ConvertedDictionaryToView.Add(Item.Key.ToString(), Item.Value);
        }

        DictionaryViewer(Header, Footer, ConvertedDictionaryToView);
    }

    public static void ListViewer<T>(string Header=null, string Footer=null, List<T> ListToView=null) {
        if (ListToView is null) return;

        int maxSize = 0;
        foreach (var Item in ListToView) {
            int size = Item.ToString().Count();
            if (size > maxSize) {
                maxSize = size;
            }
        }
        maxSize += 2;

        string topAndBottomBar = TextGeneration(maxSize + 2, "=");
        string dividerBar = $"|{TextGeneration(maxSize, "-")}|";
        string outputToPrint = $"{topAndBottomBar}\n";

        int pos = 0;
        foreach (var Item in ListToView) {
            KeyValuePair<string, string> SpacesFrontAndBack = new KeyValuePair<string, string>();
            if (IsEven(maxSize)) {
                int spacesrequired = maxSize - Item.ToString().Count();
                SpacesFrontAndBack = new KeyValuePair<string, string>(TextGeneration(spacesrequired / 2), TextGeneration(spacesrequired / 2));
            } else {
                int spacesrequired = maxSize - Item.ToString().Count();
                if (IsEven(spacesrequired)) {
                    SpacesFrontAndBack = new KeyValuePair<string, string>(TextGeneration(spacesrequired / 2), TextGeneration(spacesrequired / 2));
                } else {
                    int spacesFront = (int) (Convert.ToDouble(spacesrequired) / 2.0);
                    int spacesBack = Convert.ToInt32(Convert.ToDouble(spacesrequired) / 2.0);
                    SpacesFrontAndBack = new KeyValuePair<string, string>(TextGeneration(spacesFront), TextGeneration(spacesBack));
                }
            }
            
            outputToPrint += $"|{SpacesFrontAndBack.Key}{Item.ToString()}{SpacesFrontAndBack.Value}|\n";
            if (pos != ListToView.Count) {
                outputToPrint += $"{dividerBar}\n";
            }   
            pos++;
        }
        outputToPrint += topAndBottomBar;
        
        Console.Clear();
        if (Header is not null) {
            Console.WriteLine(Header);
        }
        Console.WriteLine(outputToPrint);
        if (Footer is not null) {
            Console.WriteLine(Footer);
        }
        Console.ReadLine();
    }

    private static bool IsEven(int input) => (input % 2 == 0) ? true : false;

    //Set to public because i want to use this somewhere else, normaly this is only meant for this Kit
    public static string TextGeneration(int amount, string text=" ") {
        string returnValue = "";
        while (returnValue.Count() != amount) {
            returnValue += text;
        }
        return returnValue;
    }

    private static string TextBoxCheck(string stringToCheck="", int? MinRequiredLength=null, int? MaxRequiredLength=null, List<string>? CantContain=null) {
        string returnValue = "";

        if (MinRequiredLength is not null) {
            if (stringToCheck.Length >= MinRequiredLength) {
                
            } else {
                returnValue += "Your awnser doesn't match the minimium required length.";
            }
        }
                    
        if (MaxRequiredLength is not null) {
            if (stringToCheck.Length > MaxRequiredLength) {
                if (returnValue == "") {
                    returnValue += "Your awnser doesn't match the exceeds the max length.";
                } else {
                    returnValue += "\nYour awnser doesn't match the exceeds the max length.";
                }
            }
        }

        if (CantContain is not null) {
            if (Contains(stringToCheck, CantContain)) {
                if (returnValue == "") {

                    returnValue += "Your awnser contains a word that is not allowed.";
                } else {
                    returnValue += "\nYour awnser contains a word that is not allowed.";
                }
            }
        }

        return returnValue;
    }

    private static bool Contains(string input, List<string> strings) {
        int ContainsWordCount = 0;

        foreach (string word in strings) {
            if (input.ToLower().Contains(word.ToLower())) {
                ContainsWordCount++;
            }
        }

        if (ContainsWordCount > 0) {
            return true;
        }
        return false;
    }

    private static bool UsedShift(ConsoleKeyInfo input) => input.Modifiers.HasFlag(ConsoleModifiers.Shift);
}