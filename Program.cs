using static System.Console;
Clear();

const int widthField = 20,
          heightField = 12,
          beginHorizontaPosFigure = widthField / 2 - 2,
          beginVerticalPosFigure = 0,
          maxLevel = 2;
bool[,] emptyFigure = new bool[4, 4];
bool[,] tetrisFigure = new bool[4, 4];
bool[,] emptyField = new bool[heightField, widthField];
for (int i = 0; i < emptyField.GetLength(0); i++)
    for (int j = 0; j < emptyField.GetLength(1); j++)
        emptyField[i, j] = false;
bool[,] gameField = emptyField;
int typeTetrisFigure = 1,
    scoresGame = 0,
    levelGame = 0,
    currentHorizontaPosFigure = beginHorizontaPosFigure,
    currentVerticalPosFigure = beginVerticalPosFigure;

bool[,] GreatStakanOnField(bool[,] changeField)
{
    changeField = emptyField;
    for (int i = 0; i < changeField.GetLength(0) - 1; i++)
    {
        changeField[i, 0] = true;
        changeField[i, changeField.GetLength(1) - 1] = true;
    }
    for (int i = 0; i < changeField.GetLength(1); i++)
        changeField[changeField.GetLength(0) - 1, i] = true;
    return changeField;
}

void PrintFigure(bool[,] printFigure)
{
    for (int i = 0; i < printFigure.GetLength(0); i++)
        for (int j = 0; j < printFigure.GetLength(1); j++)
            if (printFigure[i, j])
                gameField[currentVerticalPosFigure + i, currentHorizontaPosFigure + j] = true;


}

void ClearFigure(bool[,] clearFigure)
{
    for (int i = 0; i < clearFigure.GetLength(0); i++)
        for (int j = 0; j < clearFigure.GetLength(1); j++)
            if (clearFigure[i, j]) gameField[currentVerticalPosFigure + i, currentHorizontaPosFigure + j] = false;
}

bool[,] RotateFigureLine(bool[,] rotateFigure)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    if (rotateFigure[0, 1])
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
    if (CheckMistake(temporaryFigure, currentHorizontaPosFigure, currentVerticalPosFigure))
    {
        ClearFigure(rotateFigure);
        PrintFigure(temporaryFigure);
        return temporaryFigure;
    }
    else
        return rotateFigure;
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
    if (CheckMistake(temporaryFigure, currentHorizontaPosFigure, currentVerticalPosFigure))
    {
        ClearFigure(rotateFigure);
        PrintFigure(temporaryFigure);
        return temporaryFigure;
    }
    else
        return rotateFigure;
}

bool[,] RotateFigureLeftZetAndL(bool[,] rotateFigure)
{
    bool[,] temporaryFigure = new bool[rotateFigure.GetLength(0), rotateFigure.GetLength(1)];
    for (int i = 0; i < rotateFigure.GetLength(0) - 1; ++i)
        for (int j = 0; j < rotateFigure.GetLength(1) - 1; ++j)
            temporaryFigure[rotateFigure.GetLength(0) - j - 2, i] = rotateFigure[i, j];
    if (CheckMistake(temporaryFigure, currentHorizontaPosFigure, currentVerticalPosFigure))
    {
        ClearFigure(rotateFigure);
        PrintFigure(temporaryFigure);
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

bool CheckMistake(bool[,] compareFigure, int currentHorizontalPos, int currentVerticalPos)
{

    for (int i = 0; i < compareFigure.GetLength(0); i++)
        for (int j = 0; j < compareFigure.GetLength(1); j++)
        {
            WriteLine($"{gameField[currentVerticalPos + i, currentHorizontalPos + j]}+{compareFigure[i, j]}");
            if (gameField[currentVerticalPos + i, currentHorizontalPos + j] && compareFigure[i, j])
                return false;
        }
    return true;
}

void PrintGameField(bool[,] Field, int scores)
{
    for (int i = 0; i < Field.GetLength(0) - 1; i++)
    {
        for (int j = 1; j < Field.GetLength(1) - 1; j++)
        {
            CursorLeft = j;
            CursorTop = i;
            if (Field[i, j]) WriteLine("*");
            else WriteLine(" ");
        }
        WriteLine();
    }
    WriteLine("\n\n\n\n  Scores " + scores);
}

void PrintBeginLevel(int level, int scores)
{
    Clear();
    gameField = GreatStakanOnField(gameField);
    for (int i = 0; i < gameField.GetLength(0); i++)
    {
        for (int j = 0; j < gameField.GetLength(1); j++)
        {
            CursorLeft = j;
            CursorTop = i;
            if (gameField[i, j]) WriteLine("X");
            else WriteLine(" ");
        }
        WriteLine();
    }

    WriteLine("\n   Level " + level);
    WriteLine("\n  Scores " + scores);

}



bool gameEndLevel;

for (levelGame = 1; levelGame <= maxLevel; levelGame++)
{
    gameEndLevel = true;
    gameField = emptyField;
    PrintBeginLevel(levelGame, scoresGame);
    var b = ReadLine()!;
    while (gameEndLevel)
    {
        tetrisFigure = GreatFigure(out typeTetrisFigure);
        currentHorizontaPosFigure = beginHorizontaPosFigure;
        currentVerticalPosFigure = beginVerticalPosFigure;
        Write(CheckMistake(tetrisFigure, currentVerticalPosFigure, currentHorizontaPosFigure));
        if (CheckMistake(tetrisFigure, currentVerticalPosFigure, currentHorizontaPosFigure))
        {
            PrintFigure(tetrisFigure);
            PrintGameField(gameField, scoresGame);
        }
        var a = ReadLine()!;

        gameEndLevel = false;
    }

}





