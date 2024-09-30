using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum Shapes
{
    rectangle,
    star,
    triangle,
    heard,
    circle,
}
public enum Colors
{
    green,
    yellow,
    cyan,
    aquamarine,
    red,
    orange,
    purple,
}

public class CD //CellData
{
    public Colors upColor;
    public Shapes upShape;
    public Colors downColor;
    public Shapes downShape;

    public bool figureColor = false;
    public bool figureShape = false;
    public bool cellColor = false;
    public bool cellShape = false;

    public CD(Shapes downShape, Colors downColor, Shapes upShape, Colors upColor) 
    {
        SetUpColor(upColor);
        SetDownColor(downColor);
        SetUpShape(upShape);
        SetDownShape(downShape);
    }
    public CD(Shapes downShape, Colors downColor)
    {
        SetDownColor(downColor);
        SetDownShape(downShape);
    }

    public CD(Shapes downShape, Colors downColor, Shapes upShape)
    {
        SetUpColor(downColor);
        SetDownColor(downColor);
        SetUpShape(upShape);
        SetDownShape(downShape);
    }

    public CD(Shapes downShape, Colors downColor, Colors upColor)
    {
        SetUpColor(upColor);
        SetDownColor(downColor);
        SetUpShape(downShape);
        SetDownShape(downShape);
    }
    public CD() { }

    public void SetUpColor(Colors upColor)
    {
        figureColor = true;
        this.upColor = upColor;
    }
    public void SetDownColor(Colors downColor)
    {
        cellColor = true;
        this.downColor = downColor;
    }
    public void SetUpShape(Shapes upShape)
    {
        figureShape = true;
        this.upShape = upShape;
    }
    public void SetDownShape(Shapes downShape)
    {
        cellShape = true;
        this.downShape = downShape;
    }
}
