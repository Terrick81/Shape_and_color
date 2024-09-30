using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class cell : Forms, IPointerDownHandler
{
    [SerializeField] Figure _figure;
    protected override void UpdateShape()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = _imagesCell[_shape];
    }
    public void SetFigure(Figure figure)
    {
        _figure = figure;
    }
    public Vector2 GetAnchoredPosition()
    {
        return _rectTransform.anchoredPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_figure == null)
        {
            if (Figure.selected != null)
            {
                Figure figure = Figure.selected;
                if (figure.CheckCompatibility(_color, _shape))
                {
                    figure.Stay(_rectTransform, this);
                    _figure = figure;
                }
                else
                {
                    figure.Uncorrect();
                }
            }
        }
    }
    public void FreePlace()
    {
        _figure = null;
        if (_hiddenMode) UpdateColor(hideColor: true);
    }
}
