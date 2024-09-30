using UnityEngine;
using UnityEngine.UIElements;

public class MoveAnimate : MonoBehaviour
{
    private Vector2 _endPosition;
    private Vector2 _startPosition;
    private RectTransform _rectTransform;
    private Figure _figure;

    private float timeElapsed = 0;
    private const float duration = 0.5f;



    public void Create(Vector2 endPosition, RectTransform transform, Figure figure)
    {
        _endPosition = endPosition;
        _rectTransform = transform;
        _startPosition = _rectTransform.anchoredPosition;
        _figure = figure;
    }
    void Update()
    {
        if (_rectTransform != null)
        {
            _rectTransform.anchoredPosition = 
                Vector2.Lerp(_startPosition, _endPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            if (_rectTransform.anchoredPosition == _endPosition)
            {
                Destroy(this);
            }
        }
    }
}


