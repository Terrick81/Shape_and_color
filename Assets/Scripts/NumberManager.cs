using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Page
{
    win,
    defeat,
    game,
    menu
}
public class NumberManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _text;
    static NumberManager _objNumberManager;
    private void Start()
    {
        _objNumberManager = gameObject.GetComponent<NumberManager>();
    }

    public static void UpdateNumber(int level, Page page )
    {
        switch (page)
        {
            case Page.win:
                _objNumberManager._text[0].text = level.ToString();
                break;
            case Page.defeat:
                _objNumberManager._text[1].text = level.ToString();
                break;
            case Page.game:
                _objNumberManager._text[2].text = level.ToString();
                break;
            case Page.menu:
                _objNumberManager._text[3].text = level.ToString();
                break;
        }
    }
}
