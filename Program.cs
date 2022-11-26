using static System.Console;
Clear();

const int widthField = 26,
          heightField = 15,
          beginHorizontaPosFigure = widthField / 2 - 2,
          beginVerticalPosFigure = 0,
          maxLevel = 2,
          step = 3;

bool[,] tetrisFigure = new bool[4, 4];
bool[,] gameField = new bool[heightField, widthField];
gameField = GreatEmptyField();

int typeTetrisFigure = 1,
    scoresGame = 0,
    levelGame = 0,
    currentHorizontaPosFigure = beginHorizontaPosFigure,
    currentVerticalPosFigure = beginVerticalPosFigure,
    temporaryVerticalPosFigure = 0,
    temporaryHorizontalPosFigure = 0;
bool gameEndLevel,
     endFallingFigure;


bool[,] GreatEmptyField()
{
    for (int i = 0; i < gameField.GetLength(0); i++)
        for (int j = 0; j < gameField.GetLength(1); j++)
            gameField[i, j] = false;
    return gameField;
}

bool[,] GreatStakanOnField(bool[,] changeField)
{
    changeField = GreatEmptyField();
    for (int i = 0; i < changeField.GetLength(0) - 1 - step; i++)
    {
        changeField[i, step] = true;
        changeField[i, changeField.GetLength(1) - 1 - step] = true;
    }
    for (int i = step; i < changeField.GetLength(1) - step; i++)
        changeField[changeField.GetLength(0) - 1 - step, i] = true;
    return changeField;


}

void InsertFigureOnField(bool[,] insertFigure)
{
    for (int i = 0; i < insertFigure.GetLength(0); i++)
        for (int j = 0; j < insertFigure.GetLength(1); j++)
            if (insertFigure[i, j])
                gameField[currentVerticalPosFigure + i, currentHorizontaPosFigure + j] = true;
    PrintGameField(gameField, scoresGame);
}

void RemoveFigureFromField(bool[,] RemoveField)
{
    for (int i = 0; i < RemoveField.GetLength(0); i++)
        for (int j = 0; j < RemoveField.GetLength(1); j++)
            if (RemoveField[i, j])
                gameField[currentVerticalPosFigure + i, currentHorizontaPosFigure + j] = false;
    //PrintGameField(gameField, scoresGame);
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
    temporaryVerticalPosFigure = currentVerticalPosFigure + 1;
    temporaryHorizontalPosFigure = currentHorizontaPosFigure;
    RemoveFigureFromField(tetrisFigure);
    if (CheckMistake(tetrisFigure, currentVerticalPosFigure + 1, currentHorizontaPosFigure))
    {
        currentVerticalPosFigure += 1;
        InsertFigureOnField(tetrisFigure);
    }
    else
    {
        InsertFigureOnField(tetrisFigure);
        endFallingFigure = false;
    }
}

void MoveFigureRight()
{
    temporaryVerticalPosFigure = currentVerticalPosFigure;
    temporaryHorizontalPosFigure = currentHorizontaPosFigure + 1;
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
    temporaryVerticalPosFigure = currentVerticalPosFigure;
    temporaryHorizontalPosFigure = currentHorizontaPosFigure - 1;
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

void PrintGameField(bool[,] Field, int scores)
{
    for (int i = 0; i < Field.GetLength(0) - 1 - step; i++)
    {
        for (int j = 1 + step; j < Field.GetLength(1) - 1 - step; j++)
        {
            CursorLeft = j;
            CursorTop = i;
            if (Field[i, j]) WriteLine("*");
            else WriteLine(" ");
        }
        WriteLine();
    }
    CursorLeft = step;
    CursorTop = heightField + 3;
    WriteLine("Scores " + scores);
}

void PrintBeginLevel(int level, int scores)
{
    gameField = GreatStakanOnField(gameField);
    for (int i = 0; i < gameField.GetLength(0); i++)
    {
        for (int j = 0; j < gameField.GetLength(1); j++)
        {
            Console.CursorLeft = j;
            Console.CursorTop = i;
            if (gameField[i, j])
                Console.WriteLine("X");
            else
                Console.WriteLine(" ");
        }
        Console.WriteLine();
    }
    CursorLeft = step;
    CursorTop = heightField + 1;
    Console.WriteLine("Level " + level);
    CursorLeft = step;
    CursorTop = heightField + 3;
    Console.WriteLine("Scores " + scores);

}

void RotateAndMoveFigure(ConsoleKey key)
{
    switch (key)
    {
        case ConsoleKey.UpArrow:
            tetrisFigure = RotateFigureRight(tetrisFigure, typeTetrisFigure);
            break;
        case ConsoleKey.DownArrow:
            MoveFigureDown();
            break;
        case ConsoleKey.LeftArrow:
            MoveFigureLeft();
            break;
        case ConsoleKey.RightArrow:
            MoveFigureRight();
            break;
    }
}


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
            PrintGameField(gameField, scoresGame);
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
Console.CursorVisible = true;
timer.Dispose();



