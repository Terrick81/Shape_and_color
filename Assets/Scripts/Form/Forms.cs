using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static Forms;



public class Forms : MonoBehaviour
{
    #region Fields
    protected  const int MAX_PIXEL_SIZE = 400;
    protected  const float SCALE = 1.15f;
    protected static int SHAPES_LENGHT;
    protected static int COLOR_LENGHT;
    protected static int _outOfPlace;
    protected static bool _hiddenMode = false;

    [SerializeField] protected Shapes _shape;
    [SerializeField] protected Colors _color;
    [SerializeField] protected RectTransform _rectTransform;
    [SerializeField] protected Image _image;
    [SerializeField] protected bool _onRightPlace = false;

    protected static Dictionary<Shapes, Sprite> _imagesFigure = new Dictionary<Shapes, Sprite>();
    protected static Dictionary<Shapes, Sprite> _imagesCell = new Dictionary<Shapes, Sprite>();

    protected static Vector2 _pixelSize;
    protected static Vector2Int _sizeGrid;
    protected static Container _container;
    #endregion

    #region Get and Set
    public Colors GetColor() { return _color; }
    public Shapes GetShape() { return _shape; }
    public static int GetOutOfPlace() { return _outOfPlace; }
    public void SetColorShape(Colors cl, Shapes sp)
    {
        _color = cl;
        _shape = sp;
        UpdateColor(_hiddenMode);
        UpdateShape();
    }
    protected Color GetColorInColection(Colors cl)
    {
        switch (cl)
        {
            case Colors.green:      return new Color(0.25f, 0.64f, 0.12f);
            case Colors.yellow:     return new Color(0.84f, 0.74f, 0.11f);
            case Colors.cyan:       return new Color(0.19f, 0.62f, 0.85f);
            case Colors.aquamarine: return new Color(0.17f, 0.7f, 0.57f);
            case Colors.red:        return new Color(0.77f, 0.27f, 0.27f);
            case Colors.orange:     return new Color(0.81f, 0.48f, 0.16f);
            case Colors.purple:     return new Color(0.65f, 0.34f, 0.83f);
        }
        return Color.black;
    }
    #endregion

    public static void MyAwake()
    {
        SHAPES_LENGHT = Enum.GetNames(typeof(Shapes)).Length;
        COLOR_LENGHT = Enum.GetNames(typeof(Colors)).Length;
        _container = GameObject.Find("GameManager").GetComponent<Container>();
        for (int i = 0; i < _container.imagesFigure.Length; i++)
        {
            _imagesFigure.Add((Shapes)i, _container.imagesFigure[i]);
            _imagesCell.Add((Shapes)i, _container._imagesCell[i]);
        }
        Figure.MyAwake1();
    }
    public bool CheckCompatibility(Colors color, Shapes shape)
    {
        if (color == _color || shape == _shape)
            return true;
        else
            return false;
    }
    public bool CheckAllCompatibility(Colors color, Shapes shape)
    {
        if (color == _color && shape == _shape)
            return true;
        else
            return false;
    }
    public virtual void UpdateColor(bool hideColor = false)
    {
        if (hideColor) 
            _image.color = Color.gray;
        else
            _image.color = GetColorInColection(_color);        
    }
    protected virtual void UpdateShape()
    {
        
    }
    public static void SetParameters(int size, Vector2Int sizeGrid, int offset, bool hidden = false)
    {
        _hiddenMode = hidden;
        _outOfPlace = 0;
        if (size > MAX_PIXEL_SIZE)
            _pixelSize = new Vector2(MAX_PIXEL_SIZE, MAX_PIXEL_SIZE);
        else
            _pixelSize = new Vector2(size, size);
        _sizeGrid = sizeGrid;
    }
    public void Create(Vector2Int coord, Colors colors, Shapes shapes)
    {
        _rectTransform.localPosition = new Vector2(
            _pixelSize[0] * coord[1] - (_pixelSize[0] / 2 * (_sizeGrid[1] - 1)),
            -_pixelSize[1] * coord[0] + (_pixelSize[1] / 2 * (_sizeGrid[0] - 1)));
        _rectTransform.sizeDelta = _pixelSize * SCALE;
        SetColorShape(colors, shapes);
    }
}