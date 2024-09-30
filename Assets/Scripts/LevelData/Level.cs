using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class Level
{
    #region Fields
    private const int MAX_SIZE_Y = 8;
    private const int MAX_SIZE_X = 6;
    private const int MIN_SIZE = 2;
    private const int MIN_HIDDEN_LEVEL = 42;
    private const float CHANCE_HIDDEN_LEVEL = 0.15f;
    private const float CHANCE_SIMPLE_LEVEL = 0.06f;
    private const int MAX_LEVEL_SCALE = 100; //последний уровень после которого увеличение
                                             //карты не будет действовать
    private static int LENGHT_COLOR = Enum.GetNames(typeof(Colors)).Length;
    private static int LENGHT_SHAPES = Enum.GetNames(typeof(Shapes)).Length;

    private bool _hidden = false;
    private bool _hiddenIsPredetermined = false;
    private int _countColors;
    private int _countShapes;
    private int _seed;
    private List<Colors> _colors;
    private List<Shapes> _shapes;
    private int _sizeY;
    private int _sizeX;
    private CD[,] _levelMap;
    private int[] _countSetColors;
    #endregion

    #region Constructors
    //ручной ввод
    public Level(CD[,] levelMap)
    {
        _levelMap = levelMap;
        _sizeY = levelMap.GetLength(0);
        _sizeX = levelMap.GetLength(1);
    }

    //генераци€ уровн€ по всем параметрам
    public Level(int seed, int sizeY, int sizeX, int countColor, int countShape)
    {
        _seed = seed;
        SetSize(sizeY, sizeX);
        SetColorShapeCount(countColor, countShape);
        GenerateListColorShape();
        LevelGeneration();
    }

    //генераци€ уровн€ с ручным заданием цветов и форм
    public Level(int seed, List<Colors> colors, List<Shapes> shapes)
    {
        _seed = seed;
        GenerateSize();
        _colors = colors;
        _shapes = shapes;
        LevelGeneration();
    }

    //генераци€ уровн€ с заданием количества цветов и форм
    public Level(int seed, int countColor, int countShape)
    {
        _seed = seed;
        GenerateSize();
        SetColorShapeCount(countColor, countShape);
        GenerateListColorShape();
        LevelGeneration();
    }

    //полна€ генераци€ по сиду
    public Level(int seed)
    {
        _seed = seed;
        GenerateSize();
        GenerateColorShapeCount(seed);
        GenerateListColorShape();
        LevelGeneration();
    }
    #endregion

    public static void UnitRandom(int seed)
    {
        Random.InitState(seed);
    }
    private void SetColorShapeCount(int countColors, int countShapes)
    {
        if (countColors <= 0) countColors = 1;
        else if (countColors > LENGHT_SHAPES) countColors = LENGHT_SHAPES;

        if (countShapes <= 0) countShapes = 1;
        else if (countShapes > LENGHT_SHAPES) countShapes = LENGHT_SHAPES;

        _countColors = countColors;
        _countShapes = countShapes;

    }
    private void SetSize(int sizeY, int sizeX)
    {
        if (sizeY > MAX_SIZE_Y) sizeY = MAX_SIZE_Y;
        else if (sizeY < MIN_SIZE) sizeY = MIN_SIZE;

        if (sizeX > MAX_SIZE_X) sizeX = MAX_SIZE_X;
        else if (sizeX < MIN_SIZE) sizeX = MIN_SIZE;

        _sizeY = sizeY;
        _sizeX = sizeX;
    }
    public Vector2Int GetSize()
    {
        return new Vector2Int(_sizeY, _sizeX);
    }
    public CD[,] GetMap()
    {
        return _levelMap;
    }
    public bool GetHiddenMode()
    {
        return _hidden;
    }
    public void SetHiddenMode(bool hidden)
    {
        _hidden = hidden;
        _hiddenIsPredetermined = true;
    }
    private void GenerateSize()
    {
        float ratio = Random.Range(0.3f, 1f);
        int sizeY = MIN_SIZE;
        int sizeX = MIN_SIZE;
        
        if (_seed > MAX_LEVEL_SCALE && Random.value < CHANCE_SIMPLE_LEVEL)
        {
            sizeY = Random.Range(2, 5);
            sizeX = Random.Range(2, 5);
        }
        else
        {
            sizeY = MIN_SIZE + _seed / (MAX_LEVEL_SCALE / (MAX_SIZE_Y - MIN_SIZE));
            sizeX = Mathf.RoundToInt(sizeY * ratio);
        }
        SetSize(sizeY, sizeX);
    }
    private void GenerateColorShapeCount(int seed)
    {
        int maxPoints = 3 + seed / (MAX_LEVEL_SCALE / (LENGHT_COLOR + LENGHT_SHAPES));
        if (maxPoints > LENGHT_COLOR + LENGHT_SHAPES)
            maxPoints = LENGHT_COLOR + LENGHT_SHAPES;

        int countColors = Random.Range(2, maxPoints - 1);
        int countShapes = maxPoints - countColors;
        SetColorShapeCount(countColors, countShapes);
    }
    private void GenerateListColorShape()
    {
        _colors = Enum.GetValues(typeof(Colors)).Cast<Colors>().ToList();
        _shapes = Enum.GetValues(typeof(Shapes)).Cast<Shapes>().ToList();

        for (int i = 0; i < (LENGHT_COLOR - _countColors); i++)
            _colors.RemoveAt(Random.Range(0, LENGHT_COLOR - i));

        for (int i = 0; i < (LENGHT_SHAPES - _countShapes); i++)
            _shapes.RemoveAt(Random.Range(0, LENGHT_SHAPES - i));
    }
    private void LevelGeneration()
    {
        SetHidden();
        CD[,] map = new CD[_sizeY, _sizeX];
        List<Shapes> shapesFigure = new List<Shapes>();
        List<int[]> cellCoord = new List<int[]>();

        for (int i = 0; i < _sizeY; i++)
        {
            for (int j = 0; j < _sizeX; j++)
            {
                cellCoord.Add(new int[2] { i, j });
                map[i, j] = new CD();
                Shapes s = _shapes[Random.Range(0, _shapes.Count)];
                map[i, j].SetDownShape(s);
                shapesFigure.Add(s);

            }
        }
        for (int i = 0; i < _sizeY; i++)
        {
            for (int j = 0; j < _sizeX; j++)
            {
                int n = Random.Range(0, shapesFigure.Count);
                map[i, j].SetUpShape(shapesFigure[n]);
                shapesFigure.RemoveAt(n);
            }
        }

        List<int[]> figureCoord = new List<int[]>(cellCoord);

        if (_colors.Count == 1)
        {
            for (int i = 0; i < _sizeY; i++)
            {
                for (int j = 0; j < _sizeX; j++)
                {
                    map[i, j].SetUpColor(_colors[0]);
                    map[i, j].SetDownColor(_colors[0]);
                }
            }
            map[Random.Range(0, _sizeY), Random.Range(0, _sizeX)].figureShape = false;
            _levelMap = map;
            return;
        }

        map = PaintPair(map, cellCoord, figureCoord);
        _levelMap = map;

        if (GameManager.generationTest) GenerationTest();
        return;
    }
    public void SetHidden()
    {
        if (_hiddenIsPredetermined == true) return;
        if (_seed > MIN_HIDDEN_LEVEL)
        {
            if (Random.value < CHANCE_HIDDEN_LEVEL)
            {
                _hidden = true;
            }
        }
        _hiddenIsPredetermined = true;
    }
    private void GenerationTest()
    {
        int compatibility = 0;
        int colorless = 0;
        for (int i = 0; i < _sizeY; i++)
        {
            for (int j = 0; j < _sizeX; j++)
            {

                if (_levelMap[i, j].downColor != _levelMap[i, j].upColor
                    && _levelMap[i, j].downShape != _levelMap[i, j].upShape
                    && _levelMap[i, j].figureShape)
                    compatibility++;

                if (_levelMap[i, j].downColor == Colors.green)
                {
                    //colorless++;
                }
            }
        }
        if (colorless > 0 || compatibility > 0)
            System.IO.File.AppendAllText(
                "C:\\Users\\Terre\\OneDrive\\–абочий стол\\" + GameManager.nameFile + ".txt",
                "\n уровень: " + _seed
                + " | непокрашенных: " + colorless
                + " | несостыковок: " + compatibility
                + " | цветов: " + _countColors
                + " | форм: " + _countShapes);
    }
    private CD[,] PaintPair(CD[,] map, List<int[]> colorlessCellCoord, List<int[]> colorlessFigureCoord)
    {
        bool isPainted = false;
        bool deleteFigure = true;
        bool deleteCell;
        bool lastFigure = false;
        int[] lastFigureCoord = new int[2];
        _countSetColors = new int[LENGHT_COLOR];
        #region PaintPair
        while (colorlessCellCoord.Count > 0)
        {
            #region ChoosingCell
            deleteCell = true;
            int number = 0;
            if (lastFigure)
            {
                number = colorlessCellCoord.IndexOf(lastFigureCoord);
                if (number == -1)
                {
                    number = Random.Range(0, colorlessCellCoord.Count);
                }
                else
                    lastFigure = false;
            }
            else
            {
                number = Random.Range(0, colorlessCellCoord.Count);
            }

            CD cellData = map[colorlessCellCoord[number][0], colorlessCellCoord[number][1]];
            int[] cellCoord = colorlessCellCoord[number];
            #endregion

            #region foundPairFigure

            #region ChoosingCollors
            //выбераем палитру цветов подложки
            List<Colors> verifyColors = CellPalette(cellData);
            #endregion

            isPainted = false;
            bool relative = false;
            while (verifyColors.Count > 0)
            {
                #region ChoosingRandomCollor
                int n = Random.Range(0, verifyColors.Count);
                Colors color = verifyColors[n];
                verifyColors.RemoveAt(n);
                #endregion

                List<int[]> localFigureCoord = new List<int[]>(colorlessFigureCoord);
                while (localFigureCoord.Count > 0)
                {
                    #region ChoosingRandomFigure
                    n = Random.Range(0, localFigureCoord.Count);
                    if (cellCoord == localFigureCoord[n])
                    {
                        CD figure = map[localFigureCoord[n][0], localFigureCoord[n][1]];
                        if (cellData.downShape == figure.upShape)
                        {
                            relative = true;
                        }
                        localFigureCoord.RemoveAt(n);
                        continue;
                    }

                    CD localFigure = map[localFigureCoord[n][0], localFigureCoord[n][1]];
                    bool paint = true;
                    #endregion

                    #region CheckingRandomFigure
                    if (cellData.downShape == localFigure.upShape)
                    {
                        if (localFigure.cellColor)
                        {
                            if (localFigure.downShape == localFigure.upShape)
                            {
                                if (localFigure.downColor == color)
                                {
                                    paint = false;
                                }
                            }
                            else
                            {
                                if (localFigure.downColor != color)
                                {
                                    paint = false;
                                }
                            }
                        }
                        #region Paint
                        if (paint)
                        {
                            map[localFigureCoord[n][0], localFigureCoord[n][1]].SetUpColor(color);
                            map[cellCoord[0], cellCoord[1]].SetDownColor(color);
                            _countSetColors[(int)color] += 1;
                            colorlessFigureCoord.Remove(localFigureCoord[n]);
                            isPainted = true;
                            lastFigure = true;
                            lastFigureCoord = localFigureCoord[n];
                            break;
                        }
                        else
                        {
                            #region correcting
                            map[localFigureCoord[n][0], localFigureCoord[n][1]].SetUpShape(map[localFigureCoord[n][0], localFigureCoord[n][1]].downShape);
                            map[cellCoord[0], cellCoord[1]].SetDownShape(map[localFigureCoord[n][0], localFigureCoord[n][1]].downShape);
                            map[localFigureCoord[n][0], localFigureCoord[n][1]].SetUpColor(cellData.upColor);
                            map[cellCoord[0], cellCoord[1]].SetDownColor(cellData.upColor);
                            _countSetColors[(int)cellData.upColor] += 1;
                            colorlessFigureCoord.Remove(localFigureCoord[n]);
                            isPainted = true;
                            lastFigure = true;
                            lastFigureCoord = localFigureCoord[n];
                            break;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        lastFigure = false;
                    }
                    localFigureCoord.RemoveAt(n);
                    #endregion
                }
                if (isPainted) break;
            }
            if (!isPainted && relative)
            {
                List<int[]> localFigureCoord = new List<int[]>(colorlessFigureCoord);
                CD localCell;
                foreach (int[] Coord in colorlessCellCoord)
                {
                    if (cellCoord == Coord) continue;

                    localCell = map[Coord[0], Coord[1]];

                    if (localCell.downShape == localCell.upShape)
                    {
                        if (!localCell.figureColor)
                        {
                            map[Coord[0], Coord[1]].SetUpShape(map[cellCoord[0], cellCoord[1]].downShape);
                            map[cellCoord[0], cellCoord[1]].SetUpShape(map[Coord[0], Coord[1]].downShape);
                            Colors color1 = GetPopularColor();
                            if (map[Coord[0], Coord[1]].upShape == map[Coord[0], Coord[1]].downShape)
                            {
                                map[Coord[0], Coord[1]].SetUpColor(color1);
                                map[cellCoord[0], cellCoord[1]].SetDownColor(color1);
                                _countSetColors[(int)color1] += 1;

                                List<Colors> localColors = new List<Colors>(_colors);
                                localColors.Remove(color1);
                                int n1 = Random.Range(0, localColors.Count);

                                map[Coord[0], Coord[1]].SetDownColor(localColors[n1]);
                                _countSetColors[(int)localColors[n1]] += 1;
                                map[cellCoord[0], cellCoord[1]].SetUpColor(localColors[n1]);
                            }
                            else
                            {
                                map[Coord[0], Coord[1]].SetUpColor(color1);
                                map[Coord[0], Coord[1]].SetDownColor(color1);
                                _countSetColors[(int)color1] += 1;

                                map[cellCoord[0], cellCoord[1]].SetUpColor(color1);
                                map[cellCoord[0], cellCoord[1]].SetDownColor(color1);
                                _countSetColors[(int)color1] += 1;

                                colorlessFigureCoord.Remove(Coord);
                                colorlessCellCoord.Remove(Coord);
                                colorlessFigureCoord.Remove(cellCoord);
                                colorlessCellCoord.Remove(cellCoord);
                                deleteCell = false;
                                break;
                            }
                        }
                    }
                }
                if (deleteCell)
                {
                    Colors color = GetPopularColor();

                    map[cellCoord[0], cellCoord[1]].figureShape = false;
                    map[cellCoord[0], cellCoord[1]].SetDownColor(color);
                    _countSetColors[(int)color] += 1;
                    colorlessFigureCoord.Remove(cellCoord);
                    colorlessCellCoord.Remove(cellCoord);
                    deleteFigure = false;
                    deleteCell = false;
                }
            }
            #endregion
            if (deleteCell) colorlessCellCoord.RemoveAt(number);
        }
        #endregion
        if (deleteFigure)
        {
            CD cell;
            for (int i = 0; i < _sizeY; i++)
            {
                for (int j = 0; j < _sizeX; j++)
                {
                    cell = map[i, j];
                    if (cell.upColor == cell.downColor && cell.upShape == cell.downShape && cell.figureColor)
                    {
                        map[i, j].figureShape = false;
                        return map;
                    }
                }
            }
            map[Random.Range(0, _sizeY), Random.Range(0, _sizeX)].figureShape = false;
        }
        return map;
    }

    //выбераем палитру цветов подложки

    private Colors GetPopularColor()
    {
        int maxValue = _countSetColors.Max();
        int maxIndex = _countSetColors.ToList().IndexOf(maxValue);
        return (Colors)maxIndex;
    }

    private List<Colors> CellPalette(CD cellData)
    {
        List<Colors> verifyColors = new List<Colors>(_colors);
        if (cellData.figureColor)
        {
            if (cellData.upShape == cellData.downShape)
            {
                verifyColors.Remove(cellData.upColor);
            }
            else
            {
                verifyColors.Clear();
                verifyColors.Add(cellData.upColor);
            }
        }
        return verifyColors;
    }
}

