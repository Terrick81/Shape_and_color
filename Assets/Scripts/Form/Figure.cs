using UnityEngine;
using UnityEngine.EventSystems;
using YG;
using Image = UnityEngine.UI.Image;

public class Figure : Forms, IPointerDownHandler
{
    #region Fields
    public static Figure selected;
    private static SFXManager _sfxManager;

    [SerializeField] 
    private cell _cell;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    private MoveAnimate _moveAnimate;
    #endregion
    public static void MyAwake1()
    {
        _sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (selected == null)
        {
            SelectOn();
        }
        else
        {
            if (selected == this)
                SelectOff();
            else
            {
                selected.SelectOff();
                SelectOn();
            }
        }
    }
    private void SelectOff()
    {
        selected = null;
        _animator.SetTrigger("Not selected");
        SwithHide(true);
        //_canvasGroup.blocksRaycasts = true;
        _sfxManager.PlayDrop();
    }
        
    private void SwithHide(bool hide)
    {
        if (_hiddenMode)
        {
            UpdateColor(hide);
            _cell.UpdateColor(hide);
        }
    }
    private void SelectOn()
    {
        selected = this;
        _animator.SetTrigger("Selected");
        SwithHide(false);
        _sfxManager.PlayDrop();
    }
    public void AfterMoveAnimation()
    {
        CheckOnRightPlace();
        GameManager.TurnStatic();
    }
    public void Uncorrect()
    {
        SFXManager.Play(nameClip.uncorrect);
        _animator.SetTrigger("Uncorrect");
    }
    public void Stay(RectTransform RT, cell stopCell)
    {
        gameObject.transform.SetSiblingIndex(transform.parent.childCount);
        _cell.FreePlace();
        _cell = stopCell;
        selected = null;
        _canvasGroup.blocksRaycasts = false;
        _animator.SetTrigger("Move");
        _moveAnimate = gameObject.AddComponent<MoveAnimate>();
        _moveAnimate.Create(_cell.GetAnchoredPosition(), _rectTransform, this);
        Invoke("AfterMoveAnimation", 0.5f);
        _sfxManager.PlayTake();
    }
    private bool SetCell(cell cell)
    {
        if (cell.CheckCompatibility(_color, _shape))
        {
            _cell = cell;
            _rectTransform.anchoredPosition = _cell.GetAnchoredPosition();
            CheckOnRightPlace(start: true);
            return true;
        }
        else { return false; }
    }
    public override void UpdateColor(bool hideColor = false)
    {
        base.UpdateColor(hideColor);
        _image.color += new Color(0.1f, 0.1f, 0.1f, 1);
    }
    protected override void UpdateShape()
    {
        GetComponent<Image>().sprite = _imagesFigure[_shape];
    }
    private void CheckOnRightPlace(bool start = false)
    {
        if (_cell.CheckAllCompatibility(_color, _shape))
        {
            if (start)
            {
                _canvasGroup.alpha = 1f;
                _rectTransform.sizeDelta = _pixelSize * SCALE;            
            }
            else
            {
                SFXManager.Play(nameClip.good);
                _animator.SetBool("RightPlace", true);
                _outOfPlace--;
            }
            _canvasGroup.blocksRaycasts = false;
            _onRightPlace = true;
            Destroy(_animator, 5f);
        }
        else
        {
            if (start)
            {
                _canvasGroup.alpha = 0.93f;
                _rectTransform.sizeDelta = _pixelSize;
                _outOfPlace++;
            }
            else
            {
                SwithHide(true);
            }
            _canvasGroup.blocksRaycasts = true;
            _onRightPlace = false;
            
        }
        if (!start)
        {
            
            _animator.SetTrigger("RightPlaceTrigger");
            if (_outOfPlace == 0)
            {
                GameManager.OpenWinPageStatic();
            }
        } 
    }
    public void SwithColor()
    {
        SwithHide(false);
        GetComponent<Image>().color = GetColorInColection(_color);
    }
    public void Create(Vector2Int coord, Colors colors, Shapes shapes, cell cell)
    {
        Create(coord, colors, shapes);
        _rectTransform.sizeDelta = _pixelSize;
        SetCell(cell);
        cell.SetFigure(this);
    }
}