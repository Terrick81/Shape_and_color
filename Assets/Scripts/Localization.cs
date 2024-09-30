using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using YG;

public class Localization : MonoBehaviour
{
    [SerializeField] private Animation _animationText;
    [SerializeField] private GameObject _animationObject;
    [SerializeField] private TextMeshProUGUI _localizationText;
    [SerializeField] private TextMeshProUGUI[] _text;
    

    private const int MAX_LANGUAGE = 3;
    private const int MAX_LOCALIZATION_POSITION = 9;
    private const int MOVE_TEXT_ID = 9;

    private static Localization _localization;
    
    private int _lng;
    private string[,] _translate = new string[MAX_LOCALIZATION_POSITION, MAX_LANGUAGE]
    {
        {"уровень", "level", "seviyesi" },
        {"Русский", "English", "Türk" },
        {"загрузка...", "loading...", "yükleniyor..." },
        {"уровень", "level", "seviyesi" },
        {"уровень \r\nпройден", "level \r\nis passed", "seviye \r\nasıldı" },
        {"уровень \r\nпровален", "level \r\nis not passed", "seviye \r\ngecilmedi"},
        {"закончились ходы\r\n\r\nпродолжить за рекламу?", "have you run out of moves\r\n\r\ncontinue for advertising?", "reklamlar icin devam etmek icin\r\n\r\nhamleleri bitti mi?"},
        {"таблица лидеров", "leaderboard", "liderlik tablosu"},
        {"вы не авторизованы\r\n\r\nвойти в профиль?", "are you not logged\r\n\r\nin to your profile?", "yetkili değil misiniz\r\n\r\nprofile girmek icin?"},
    };

    private void Start()
    {
        _localization = GetComponent<Localization>();
        _lng = SavesYG.GetLanguage();
        TranslatePosition();
    }

    public static int GetLng()
    {
        return _localization._lng;
    }
    public void SwithLanguage()
    {
        _animationObject.SetActive(true);
        UnityEngine.Debug.Log(SavesYG.GetLanguage());
        _lng = (SavesYG.GetLanguage() + 1) % MAX_LANGUAGE;
        _localizationText.text = _translate[1, _lng];
        _animationText.Stop();
        _animationText.Play();
        SavesYG.SetLanguage(_lng);
        TranslatePosition();
        TurnText(GameManager.GetTurn());

    }

    public static void TurnTextStatic(int turn)
    {
        _localization.TurnText(turn);
    }

    private void OnDisable()
    {
        _animationObject.SetActive(false);
    }

    public void TurnText(int turn)
    {
        switch (_lng)
        {
            case 0:
                if (turn == 1) _text[MOVE_TEXT_ID].text = "ход";
                else if (turn > 1 && turn < 5) _text[MOVE_TEXT_ID].text = "ходa";
                else _text[MOVE_TEXT_ID].text = "ходов";
                break;
            case 1:
                if (turn == 0) _text[MOVE_TEXT_ID].text = "move";
                else  _text[MOVE_TEXT_ID].text = "moves";
                break;
            case 2:
                _text[MOVE_TEXT_ID].text = "hamle";
                break;
        }
    }

    private void TranslatePosition()
    {
        for (int i = 0; i < MAX_LOCALIZATION_POSITION; i++)
        {
            _text[i].text = _translate[i, _lng];
        }
    }

}
