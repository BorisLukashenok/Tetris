
Console.Clear();

const int indentFromTheLeft = 25,  // отступ от левого края терминала
          widthField = 26,
          heightField = 15,
          beginHorizontaPosFigure = widthField / 2 - 2,
          beginVerticalPosFigure = 0,
          maxLevel = 2,
          step = 3; // Отступ от границ массива, чтоб при проверки воможности наложения фигуры на поле 
                    // не заходить за границы массива с игровым полем

const char simbolBlock = '█',
           simbolArrowRight = '→',
           simbolArrowLeft='←',
           simbolArrowDown ='↓',
           simbolArrowUp='↑';


bool[,] tetrisFigure = new bool[4, 4];
bool[,] gameField = new bool[heightField, widthField];
gameField = GreatEmptyField();

int typeTetrisFigure = 1,
    scoresGame = 0,
    levelGame = 0,
    currentHorizontaPosFigure = beginHorizontaPosFigure,
    currentVerticalPosFigure = beginVerticalPosFigure;
    
bool gameEndLevel,
     endFallingFigure,
     switchAccessToField = true; // Доступ к игровому полю(gameField), чтоб функции по нажатию клавиши
                                 // и по таймеру, не изменяли его одновременно

System.Timers.Timer timer = new(interval: 2000);
timer.Elapsed += (sender, e) => MoveFigureDown();

Console.CursorVisible = false;
for (levelGame = 1; levelGame <= maxLevel; levelGame++)
{
    gameEndLevel = true;
    endFallingFigure = true;
    PrintBeginLevel(levelGame, scoresGame);
    while (gameEndLevel)
    {
        tetrisFigure = GreatFigure(out typeTetrisFigure);
        currentHorizontaPosFigure = beginHorizontaPosFigure;
        currentVerticalPosFigure = beginVerticalPosFigure;
        timer.Start();
        if (CheckMistake(tetrisFigure, currentVerticalPosFigure, currentHorizontaPosFigure))
        {
            InsertFigureOnField(tetrisFigure);
            PrintGameField();
            endFallingFigure = true;
            while (endFallingFigure)
            {
                if (Console.KeyAvailable)
                    RotateAndMoveFigure(Console.ReadKey(true).Key);                
            }
        }
        else
        {
            timer.Stop();
            gameEndLevel = false;
        }
    }

}
Console.ResetColor();
Console.CursorVisible = true;
timer.Dispose();
Console.Clear();

// Конец программы

bool[,] GreatEmptyField()
{
    for (int i = 0; i < gameField.GetLength(0); i++)
        for (int j = 0; j < gameField.GetLength(1); j++)
            gameField[i, j] = false;
    return gameField;
}

bool[,] GreatStakanOnField(bool[,] gameField)
{
    gameField = GreatEmptyField();
    for (int i = 0; i < gameField.GetLength(0) - 1 - step; i++)
    {
        gameField[i, step] = true;
        gameField[i, gameField.GetLength(1) - 1 - step] = true;
    }
    for (int i = step; i < gameField.GetLength(1) - step; i++)
        gameField[gameField.GetLength(0) - 1 - step, i] = true;
    return gameField;


}

void InsertFigureOnField(bool[,] insertFigure)
{
    ChangeAccess();
    for (int i = 0; i < insertFigure.GetLength(0); i++)
        for (int j = 0; j < insertFigure.GetLength(1); j++)
            if (insertFigure[i, j])
                gameField[currentVerticalPosFigure + i, currentHorizontaPosFigure + j] = true;
    PrintGameField();
    ChangeAccess();
}

void RemoveFigureFromField(bool[,] RemoveField)
{
    ChangeAccess();
    for (int i = 0; i < RemoveField.GetLength(0); i++)
        for (int j = 0; j < RemoveField.GetLength(1); j++)
            if (RemoveField[i, j])
                gameField[currentVerticalPosFigure + i, currentHorizontaPosFigure + j] = false;
    //PrintGameField(gameField, scoresGame);
    ChangeAccess();
}

bool[,] RotateFigureLine(bool[,] rotateFigure)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    if (rotateFigure[1, 0])
    {
        for (int i = 0; i < rotateFigure.GetLength(1); i++)
            temporaryFigure[1, i] = false;
        for (int i = 0; i < rotateFigure.GetLength(0); i++)
            temporaryFigure[i, 1] = true;
    }
    else
    {
        for (int i = 0; i < rotateFigure.GetLength(0); i++)
            temporaryFigure[i, 1] = false;
        for (int i = 0; i < rotateFigure.GetLength(1); i++)
            temporaryFigure[1, i] = true;
    }
    RemoveFigureFromField(rotateFigure);
    if (CheckMistake(temporaryFigure, currentVerticalPosFigure, currentHorizontaPosFigure))
    {
        InsertFigureOnField(temporaryFigure);
        return temporaryFigure;
    }
    else
    {
        InsertFigureOnField(rotateFigure);
        return rotateFigure;
    }
}

bool[,] RotateFigureSquare(bool[,] rotateFigure)
{
    return rotateFigure;
}

bool[,] RotateFigureRightZetAndL(bool[,] rotateFigure)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    for (int i = 0; i < rotateFigure.GetLength(0) - 1; ++i)
        for (int j = 0; j < rotateFigure.GetLength(1) - 1; ++j)
            temporaryFigure[i, j] = rotateFigure[rotateFigure.GetLength(0) - j - 2, i];
    RemoveFigureFromField(rotateFigure);
    if (CheckMistake(temporaryFigure, currentVerticalPosFigure, currentHorizontaPosFigure))
    {

        InsertFigureOnField(temporaryFigure);
        return temporaryFigure;
    }
    else
    {
        InsertFigureOnField(rotateFigure);
        return rotateFigure;
    }
}

bool[,] RotateFigureLeftZetAndL(bool[,] rotateFigure)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    for (int i = 0; i < rotateFigure.GetLength(0) - 1; ++i)
        for (int j = 0; j < rotateFigure.GetLength(1) - 1; ++j)
            temporaryFigure[rotateFigure.GetLength(0) - j - 2, i] = rotateFigure[i, j];
    if (CheckMistake(temporaryFigure, currentVerticalPosFigure, currentHorizontaPosFigure))
    {
        RemoveFigureFromField(rotateFigure);
        InsertFigureOnField(temporaryFigure);
        return temporaryFigure;
    }
    else
        return rotateFigure;
}

bool[,] RotateFigureRight(bool[,] rotateFigure, int typeRotateFiguree)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    switch (typeRotateFiguree)
    {
        case 1:
            {
                temporaryFigure = RotateFigureLine(rotateFigure);
                break;
            }
        case 2:
            {
                temporaryFigure = RotateFigureSquare(rotateFigure);
                break;
            }
        default:
            {
                temporaryFigure = RotateFigureRightZetAndL(rotateFigure);
                break;
            }
    }
    return temporaryFigure;
}

bool[,] RotateFigureLeft(bool[,] rotateFigure, int typeRotateFiguree)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    switch (typeRotateFiguree)
    {
        case 1:
            {
                temporaryFigure = RotateFigureLine(rotateFigure);
                break;
            }
        case 2:
            {
                temporaryFigure = RotateFigureSquare(rotateFigure);
                break;
            }
        default:
            {
                temporaryFigure = RotateFigureLeftZetAndL(rotateFigure);
                break;
            }
    }
    return temporaryFigure;
}

void MoveFigureDown()
{
    CheckedAndWiteAccess();
    RemoveFigureFromField(tetrisFigure);
    if (CheckMistake(tetrisFigure, currentVerticalPosFigure + 1, currentHorizontaPosFigure))
    {
        currentVerticalPosFigure += 1;
        InsertFigureOnField(tetrisFigure);
    }
    else
    {
        InsertFigureOnField(tetrisFigure);
        VerifyFullLine();
        endFallingFigure = false;
    }
}

void MoveFigureRight()
{
    RemoveFigureFromField(tetrisFigure);
    if (CheckMistake(tetrisFigure, currentVerticalPosFigure, currentHorizontaPosFigure + 1))
    {
        currentHorizontaPosFigure += 1;
        InsertFigureOnField(tetrisFigure);
    }
    else
    {
        InsertFigureOnField(tetrisFigure);
    }
}

void MoveFigureLeft()
{    
    RemoveFigureFromField(tetrisFigure);
    if (CheckMistake(tetrisFigure, currentVerticalPosFigure, currentHorizontaPosFigure - 1))
    {
        currentHorizontaPosFigure -= 1;
        InsertFigureOnField(tetrisFigure);
    }
    else
    {
        InsertFigureOnField(tetrisFigure);
    }
}

bool[,] GreatFigure(out int newTypeFigure)
{
    bool[,] newGreatFigure = new bool[4, 4];
    newTypeFigure = new Random().Next(1, 8);
    switch (newTypeFigure)
    {
        case 1: //линия
            {
                for (int i = 0; i < 4; i++)
                    newGreatFigure[1, i] = true;
                break;
            }

        case 2: //квадрат
            {
                for (int i = 1; i < 3; i++)
                    for (int j = 1; j < 3; j++)
                        newGreatFigure[i, j] = true;
                break;
            }

        case 3: //L правая
            {
                for (int i = 0; i < 3; i++)
                    newGreatFigure[0, i] = true;
                newGreatFigure[1, 2] = true;
                break;
            }

        case 4: // L левая
            {
                for (int i = 0; i < 3; i++)
                    newGreatFigure[0, i] = true;
                newGreatFigure[1, 0] = true;
                break;
            }

        case 5: // Пирамида
            {
                for (int i = 0; i < 3; i++)
                    newGreatFigure[1, i] = true;
                newGreatFigure[0, 1] = true;
                break;
            }

        case 6: // Z правая
            {
                newGreatFigure[0, 0] = true; newGreatFigure[1, 0] = true;
                newGreatFigure[1, 1] = true; newGreatFigure[2, 1] = true;
                break;
            }

        case 7: // Z левая
            {
                newGreatFigure[0, 1] = true; newGreatFigure[1, 0] = true;
                newGreatFigure[1, 1] = true; newGreatFigure[2, 0] = true;
                break;
            }
    }
    return newGreatFigure;
}

bool CheckMistake(bool[,] compareFigure, int currentVerticalPos, int currentHorizontalPos)
{

    for (int i = 0; i < compareFigure.GetLength(0); i++)
        for (int j = 0; j < compareFigure.GetLength(1); j++)
            if (gameField[currentVerticalPos + i, currentHorizontalPos + j] && compareFigure[i, j])
                return false;

    return true;
}

void PrintGameField()
{
    Console.ForegroundColor = ConsoleColor.Green;
    for (int i = 0; i < gameField.GetLength(0) - 1 - step; i++)
    {
        for (int j = 1 + step; j < gameField.GetLength(1) - 1 - step; j++)
        {
            Console.CursorLeft = j + indentFromTheLeft;
            Console.CursorTop = i;
            if (gameField[i, j])
                Console.WriteLine(simbolBlock);
            else
                Console.WriteLine(" ");
        }
        Console.WriteLine();
    }
    Console.CursorLeft = step + indentFromTheLeft;
    Console.CursorTop = heightField + 2;
    Console.WriteLine("Scores " + scoresGame);
    Console.CursorVisible = false;
}

void PrintBeginLevel(int level, int scores)
{
    gameField = GreatStakanOnField(gameField);
    Console.ForegroundColor = ConsoleColor.Blue;
    for (int i = 0; i < gameField.GetLength(0); i++)
    {
        for (int j = 0; j < gameField.GetLength(1); j++)
        {
            Console.CursorLeft = j + indentFromTheLeft;
            Console.CursorTop = i;
            if (gameField[i, j])
                Console.WriteLine(simbolBlock);
            else
                Console.WriteLine(" ");
        }
        Console.WriteLine();
    }
    Console.ForegroundColor = ConsoleColor.DarkYellow;

    Console.CursorLeft = step + indentFromTheLeft;
    Console.CursorTop = heightField;
    Console.WriteLine("Level " + level);

    Console.CursorLeft = step + indentFromTheLeft;
    Console.CursorTop = heightField + 2;
    Console.WriteLine("Scores " + scores);
  
    Console.CursorLeft = step + indentFromTheLeft;
    Console.CursorTop = heightField + 2;
    Console.WriteLine("Scores " + scores);

    Console.CursorLeft = widthField + indentFromTheLeft;
    Console.CursorTop = heightField - 14;
    Console.WriteLine($"{simbolArrowUp} and W - ROTATE FIGURE");

    Console.CursorLeft = widthField + indentFromTheLeft;
    Console.CursorTop = heightField - 12;
    Console.WriteLine($"{simbolArrowRight} and A - MOVE FIGURE LEFT");

    Console.CursorLeft = widthField + indentFromTheLeft;
    Console.CursorTop = heightField - 10;
    Console.WriteLine($"{simbolArrowRight} and D - MOVE FIGURE RIGHT");

    Console.CursorLeft = widthField + indentFromTheLeft;
    Console.CursorTop = heightField - 8;
    Console.WriteLine($"{simbolArrowDown} and S - MOVE FIGURE SPEED DOWN");

    Console.ResetColor();
}

void RotateAndMoveFigure(ConsoleKey key)
{
    CheckedAndWiteAccess();
    switch (key)
    {
        case ConsoleKey.W:
        case ConsoleKey.UpArrow:
            tetrisFigure = RotateFigureRight(tetrisFigure, typeTetrisFigure);
            break;
        case ConsoleKey.S:   
        case ConsoleKey.DownArrow:
            MoveFigureDown();
            break;
        case ConsoleKey.A:    
        case ConsoleKey.LeftArrow:
            MoveFigureLeft();
            break;
        case ConsoleKey.D:    
        case ConsoleKey.RightArrow:
            MoveFigureRight();
            break;
    }
}

void ChangeAccess()
{
    switchAccessToField = !switchAccessToField;
}

void CheckedAndWiteAccess()
{
    if (!switchAccessToField)
        while (true)
        {
            if (switchAccessToField)
                break;
            else
                Thread.Sleep(250);
        }
}

void ShiftField(int endShift, int deleteLine)
{
    for (int i = deleteLine; i > endShift; i--)
    {
        for (int j = 1 + step; j < gameField.GetLength(1) - 1 - step; j++)
            gameField[i, j] = gameField[i - 1, j];
    }
    for (int k = 1 + step; k < gameField.GetLength(1) - 1 - step; k++)
        gameField[endShift, k] = false;

}

void VerifyFullLine()  
{
    int posFirstNoEmptyString = 0;
    bool firstNoEmptyString = true,
         checkedFullLine;
    for (int i = 0; i < gameField.GetLength(0) - 1 - step; i++)
    {
        checkedFullLine = true;
        for (int j = 1 + step; j < gameField.GetLength(1) - 1 - step; j++)
        {
            if (!gameField[i, j])
                checkedFullLine = false;
            else
            {
                if (firstNoEmptyString)
                {
                    firstNoEmptyString = false;
                    posFirstNoEmptyString = i;
                }
            }
        }
        if (checkedFullLine)
        {
            ShiftField(posFirstNoEmptyString, i);
            scoresGame += 10;
        }
    }
}